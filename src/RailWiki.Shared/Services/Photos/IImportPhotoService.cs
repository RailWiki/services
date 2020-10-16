using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Geography;
using RailWiki.Shared.Entities.Photos;
using RailWiki.Shared.Entities.Roster;
using RailWiki.Shared.Entities.Users;
using RailWiki.Shared.Models.Photos;
using RailWiki.Shared.Services.Roster;
using RailWiki.Shared.Services.Users;

namespace RailWiki.Shared.Services.Photos
{
    public interface IImportPhotoService
    {
        Task ImportPhotoAsync(ImportPhotoModel importModel);
    }

    public class ImportPhotoService : IImportPhotoService
    {
        private const string LookupSourceKey = "RRPA";

        private readonly IPhotoService _photoService;
        private readonly IRepository<Location> _locationRepository;
        private readonly IUserService _userService;
        private readonly ILocomotiveService _locomotiveService;
        private readonly IRoadService _roadService;
        private readonly IRepository<Album> _albumRepository;
        private readonly ICrossReferenceService _crossReferenceService;
        private readonly IRepository<StateProvince> _stateProvinceRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ImportPhotoService> _logger;

        public ImportPhotoService(IPhotoService photoService,
            IRepository<Location> locationRepository,
            IUserService userService,
            ILocomotiveService locomotiveService,
            IRoadService roadService,
            IRepository<Album> albumRepository,
            ICrossReferenceService crossReferenceService,
            IRepository<StateProvince> stateProvinceRepository,
            IHttpClientFactory httpClientFactory,
            ILogger<ImportPhotoService> logger)
        {
            _photoService = photoService;
            _locationRepository = locationRepository;
            _userService = userService;
            _locomotiveService = locomotiveService;
            _roadService = roadService;
            _albumRepository = albumRepository;
            _crossReferenceService = crossReferenceService;
            _stateProvinceRepository = stateProvinceRepository;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task ImportPhotoAsync(ImportPhotoModel importModel)
        {
            var photo = new Photo
            {
                Title = importModel.Title,
                Description = importModel.Description,
                PhotoDate = importModel.PhotoDate,
                UploadDate = importModel.UploadDate,
                Author = importModel.Author,
                //Filename = // Set below
            };

            photo.UserId = await GetOrCreateUserAsync(importModel.CollectionOf);
            photo.AlbumId = await GetOrCreateAlbumAsync(photo.UserId, importModel.Album);

            var location = await GetOrCreateLocationAsync(importModel.Location);
            photo.LocationId = location?.Id;

            if (importModel.Locomotives?.Any() ?? false)
            {
                foreach(var locoModel in importModel.Locomotives)
                {
                    var locomotive = await GetOrCreateLocomotiveAsync(locoModel);
                    if (locomotive != null)
                    {
                        var photoLocomotive = new PhotoLocomotive
                        {
                            Photo = photo,
                            LocomotiveId = locomotive.Id
                        };
                        photo.Locomotives.Add(photoLocomotive);
                    }
                }
            }

            if (!importModel.ImageFileUrl.StartsWith("http"))
            {
                // Assume the photo is on the same domain as RRPA
                importModel.ImageFileUrl = $"http://rrpicturearchives.net/{importModel.ImageFileUrl}";
            }

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var imageResponse = await httpClient.GetAsync(importModel.ImageFileUrl);
                if (!imageResponse.IsSuccessStatusCode)
                {
                    _logger.LogError($"Could not retrieve image from {importModel.ImageFileUrl}. Will not save photo.");
                    throw new Exception($"Error retrieving image file from '{importModel.ImageFileUrl}' to save");
                }

                var imageBytes = await imageResponse.Content.ReadAsByteArrayAsync();
                var contentType = imageResponse.Content.Headers.ContentType.MediaType;
                var origFileName = Path.GetFileName(importModel.ImageFileUrl);

                photo.Filename = await _photoService.SavePhotoFileAsync(photo.AlbumId, imageBytes, origFileName, contentType);
            }

            // TODO: Add photo categories

            await _photoService.CreateAsync(photo);
        }

        private async Task<int> GetOrCreateUserAsync(ImportGenericLookupModel userModel)
        {
            var userId = await _crossReferenceService.GetEntityIdAsync(typeof(User), LookupSourceKey, userModel.RefId);
            if (!userId.HasValue)
            {
                var userNameParts = userModel.Name.Split(' ');

                var user = new User
                {
                    EmailAddress = "unk",
                    FirstName = userNameParts[0],
                    LastName = userNameParts[1],
                    RegisteredOn = DateTime.UtcNow
                };
                await _userService.CreateUserAsync(user);

                await _crossReferenceService.CreateAsync(typeof(User), user.Id, LookupSourceKey, userModel.RefId);

                userId = user.Id;
            }

            return userId.Value;
        }

        private async Task<int> GetOrCreateAlbumAsync(int userId, ImportGenericLookupModel albumModel)
        {
            var albumId = await _crossReferenceService.GetEntityIdAsync(typeof(Album), LookupSourceKey, albumModel.RefId);
            if (!albumId.HasValue)
            {
                var album = new Album
                {
                    UserId = userId,
                    Description = "imported",
                    Title = albumModel.Name,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow
                };
                await _albumRepository.CreateAsync(album);

                await _crossReferenceService.CreateAsync(typeof(Album), album.Id, LookupSourceKey, albumModel.RefId);
                albumId = album.Id;
            }

            return albumId.Value;
        }

        private async Task<Location> GetOrCreateLocationAsync(ImportGenericLookupModel locationModel)
        {
            Location location = null;

            var locationId = await _crossReferenceService.GetEntityIdAsync(typeof(Location), LookupSourceKey, locationModel.RefId);
            if (locationId.HasValue)
            {
                location = await _locationRepository.TableNoTracking.FirstOrDefaultAsync(x => x.Name == locationModel.Name);
            }

            if (location == null)
            {
                var locNameParts = locationModel.Name.Split(',');

                // This isn't the most reliable, but hopefully will get us a good start...                
                if (locNameParts.Count() != 2)
                {
                    _logger.LogInformation($"Location name '{locationModel.Name}` cannot be parsed to be created as a location.");
                    return null;
                }

                var stateProvince = await _stateProvinceRepository.Table.FirstOrDefaultAsync(x => x.Abbreviation == locNameParts[1].ToUpper().Trim());
                if (stateProvince == null)
                {
                    // TODO: Maybe it's better to have a "default" state that we can assign to
                    _logger.LogInformation($"Cannot find StateProvince for '{locNameParts[1].ToUpper()}");
                    return null;
                }

                location = new Location
                {
                    Name = locationModel.Name,
                    StateProvinceId = stateProvince.Id
                };
                await _locationRepository.CreateAsync(location);
            
                await _crossReferenceService.CreateAsync(typeof(Location), location.Id, LookupSourceKey, locationModel.RefId);
            }

            return location;
        }

        private async Task<Locomotive> GetOrCreateLocomotiveAsync(ImportLocomotiveModel locoModel)
        {
            // TODO: Move this to the LocomotiveService 
            var locomotive = await _locomotiveService.FindByRoadNumberAsync(locoModel.ReportingMarks);
            if (locomotive == null)
            {
                var locoRoadNumber = locoModel.ReportingMarks.Split(' ');

                var roadRptMarks = locoRoadNumber[0];
                var road = await _roadService.GetByReportingMarksAsync(roadRptMarks);
                if (road == null)
                {
                    _logger.LogInformation($"Could not find road with reporting marks '{roadRptMarks}'. Will not create new locomotive for import.");
                    return null;
                }

                locomotive = new Locomotive
                {
                    RoadId = road.Id,
                    RoadNumber = locoRoadNumber[1],
                    ReportingMarks = $"{road.ReportingMarks} {locoRoadNumber[1]}",
                    ModelNumber = locoModel.Model,
                    Notes = $"RRPA Ref ID: {locoModel.RefId}"
                };

                await _locomotiveService.CreateAsync(locomotive);
            }

            return locomotive;
        }
    }
}
