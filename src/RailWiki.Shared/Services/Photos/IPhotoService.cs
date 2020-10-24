using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RailWiki.Shared.Configuration;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Photos;
using RailWiki.Shared.Models.Photos;
using RailWiki.Shared.Services.FileStorage;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace RailWiki.Shared.Services.Photos
{
    public interface IPhotoService
    {
        /// <summary>
        /// Gets a photo entity by ID
        /// </summary>
        /// <param name="id">ID of photo</param>
        /// <returns>The photo entity, if foound</returns>
        Task<Photo> GetEntityByIdAsync(int id);

        /// <summary>
        /// Get's a photo response by ID
        /// </summary>
        /// <param name="id">ID of the photo</param>
        /// <returns>A <see cref="GetPhotoModel"/></returns>
        Task<GetPhotoModel> GetByIdAsync(int id);

        Task<IEnumerable<GetPhotoModel>> GetByAlbumIdAsync(int albumId);
        Task<IEnumerable<GetPhotoModel>> GetByIdsAsync(IEnumerable<int> ids);

        Task<IEnumerable<GetPhotoModel>> GetLatestAsync(int maxCount = 10);

        Task<GetPhotoModel> SavePhotoAsync(Album album, byte[] bytes, string origFileName, string contentType, bool resize = true);
        Task<string> SavePhotoFileAsync(int albumId, byte[] imageBytes, string origFileName, string contentType, bool resize = true);

        Task CreateAsync(Photo photo);
        Task UpdateAsync(Photo photo);
        Task DeleteAsync(Photo photo);
    }

    public class PhotoService : IPhotoService
    {
        private readonly IFileService _fileService;
        private readonly IRepository<Photo> _photoRepository;
        private readonly IFilePathHelper _filePathHelper;
        private readonly IMapper _mapper;
        private readonly ImageConfig _imageConfig;
        private readonly ILogger<PhotoService> _logger;

        public PhotoService(IFileService fileService,
            IRepository<Photo> photoRepository,
            IFilePathHelper filePathHelper,
            IMapper mapper,
            IOptions<ImageConfig> imageOptions,
            ILogger<PhotoService> logger)
        {
            _fileService = fileService;
            _photoRepository = photoRepository;
            _filePathHelper = filePathHelper;
            _mapper = mapper;
            _imageConfig = imageOptions.Value;
            _logger = logger;
        }

        public async Task<Photo> GetEntityByIdAsync(int id)
        {
            var photo = await _photoRepository.TableNoTracking
                   .Include(x => x.Location)
                   .Include(x => x.Album)
                   .Where(x => x.Id == id)
                   .SingleOrDefaultAsync();

            return photo;
        }

        public async Task<GetPhotoModel> GetByIdAsync(int id)
        {
            var photo = await _photoRepository.TableNoTracking
                   .Include(x => x.Location)
                   .Include(x => x.Album)
                   .Include(x => x.User)
                   .Where(x => x.Id == id)
                   .SingleOrDefaultAsync();

            var result = ToModel(photo);

            return result;
        }

        public async Task<IEnumerable<GetPhotoModel>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var photos = await _photoRepository.TableNoTracking
                .Include(x => x.Location)
                .Include(x => x.Album)
                .Include(x => x.User)
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            // TODO: probably need to add pagination

            var result = photos.Select(ToModel).ToList();
            return result;
        }

        public async Task<IEnumerable<GetPhotoModel>> GetLatestAsync(int maxCount = 10)
        {
            var photos = await _photoRepository.TableNoTracking
                .Include(x => x.Location)
                .Include(x => x.Album)
                .Include(x => x.User)
                .OrderByDescending(x => x.UploadDate)
                .Take(maxCount)
                .ToListAsync();

            var result = photos.Select(ToModel).ToList();
            return result;
        }

        public async Task<IEnumerable<GetPhotoModel>> GetByAlbumIdAsync(int albumId)
        {
            var photos = await _photoRepository.TableNoTracking
                .Include(x => x.Location)
                .Include(x => x.Album)
                .Where(x => x.AlbumId == albumId)
                .OrderByDescending(x => x.UploadDate)
                .ToListAsync(); // Need to enumerate before projecting

            var result = photos.Select(x => ToModel(x));

            return result;
        }

        public async Task<GetPhotoModel> SavePhotoAsync(Album album, byte[] bytes, string origFileName, string contentType, bool resize = true)
        {
            var saveFileName = await SavePhotoFileAsync(album.Id, bytes, origFileName, contentType, resize);

            // Now that the photo is saved and resizes complete, add to DB
            var photo = new Photo
            {
                AlbumId = album.Id,
                UserId = album.UserId, // Not sure if we'd ever want this to be a different user id?
                Filename = saveFileName,
                UploadDate = DateTime.UtcNow,
                Title = origFileName
            };

            await _photoRepository.CreateAsync(photo);

            var result = ToModel(photo);
            return result;
        }

        public async Task<string> SavePhotoFileAsync(int albumId, byte[] imageBytes, string origFileName, string contentType, bool resize = true)
        {
            var extension = Path.GetExtension(origFileName);

            var saveFileName = $"{Guid.NewGuid()}{extension}";

            _logger.LogDebug($"Saving photo to album ID {albumId}");

            // Save the original [renamed] file
            using (var stream = new MemoryStream(imageBytes))
            {
                var fileName = _filePathHelper.ResolveFilePath(albumId, saveFileName);
                await _fileService.SaveFileAsync(fileName, contentType, stream);

                _logger.LogDebug($"Saved original file to {fileName}");
            }

            if (resize)
            {
                foreach (var sizeProfile in _imageConfig.SizeProfiles)
                {
                    using (var stream = new MemoryStream(imageBytes))
                    {
                        IImageFormat imageFormat;
                        using (var image = Image.Load(stream, out imageFormat))
                        {
                            var filePath = _filePathHelper.ResolveFilePath(albumId, saveFileName, sizeProfile.Key);

                            using (var outStream = new MemoryStream())
                            {
                                image.Mutate(x => x.Resize(new ResizeOptions
                                {
                                    Mode = ResizeMode.Crop, // TODO: allow each profile to specify method used
                                    Position = AnchorPositionMode.Center,
                                    Size = new Size(sizeProfile.Width, sizeProfile.Height)
                                })
                                .AutoOrient());
                                image.Save(outStream, imageFormat);

                                await _fileService.SaveFileAsync(filePath, contentType, outStream);
                            }
                        }
                    }
                }
            }

            return saveFileName;
        }

        public async Task CreateAsync(Photo photo) =>
            await _photoRepository.CreateAsync(photo);

        public async Task UpdateAsync(Photo photo) =>
            await _photoRepository.UpdateAsync(photo);

        public async Task DeleteAsync(Photo photo)
        {
            _logger.LogDebug($"Deleting photo {photo.Id} from album {photo.AlbumId}");

            // First delete the record from the db
            await _photoRepository.DeleteAsync(photo);

            // Then delete the original and resized images
            var originalFilePath = _filePathHelper.ResolveFilePath(photo.AlbumId, photo.Filename);

            await _fileService.DeleteFileAsync(originalFilePath);
            _logger.LogDebug($"Deleted photo file from storage: {originalFilePath} (ID: {photo.Id})");

            foreach(var size in _imageConfig.SizeProfiles)
            {
                var sizeFilePath = _filePathHelper.ResolveFilePath(photo.AlbumId, photo.Filename, size.Key);
                await _fileService.DeleteFileAsync(sizeFilePath);

                _logger.LogDebug($"Deleted photo file from storage: {originalFilePath} (ID: {photo.Id})");
            }
        }

        private GetPhotoModel ToModel(Photo photo)
        {
            var model = _mapper.Map<GetPhotoModel>(photo);

            model.Files.Add("original", _fileService.ResolveFileUrl(_filePathHelper.ResolveFilePath(photo.AlbumId, photo.Filename)));
            foreach (var size in _imageConfig.SizeProfiles)
            {
                var path = _filePathHelper.ResolveFilePath(photo.AlbumId, photo.Filename, size.Key);

                var url = _fileService.ResolveFileUrl(path);
                model.Files.Add(size.Key, url);
            }

            return model;
        }
    }
}
