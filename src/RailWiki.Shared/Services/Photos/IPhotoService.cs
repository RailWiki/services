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
        /// <returns>A <see cref="PhotoResponseModel"/></returns>
        Task<PhotoResponseModel> GetByIdAsync(int id);

        Task<IEnumerable<PhotoResponseModel>> GetByAlbumIdAsync(int albumId);
        Task<IEnumerable<PhotoResponseModel>> GetByIdsAsync(IEnumerable<int> ids);

        Task<PhotoResponseModel> SavePhotoAsync(Album album, byte[] bytes, string origFileName, string contentType, bool resize = true);
        Task<string> SavePhotoFileAsync(int albumId, byte[] imageBytes, string origFileName, string contentType, bool resize = true);

        Task CreateAsync(Photo photo);
        Task UpdateAsync(Photo photo);
        Task DeleteAsync(Photo photo);
    }

    public class PhotoService : IPhotoService
    {
        private readonly IFileService _fileService;
        private readonly IRepository<Photo> _photoRepository;
        private readonly IMapper _mapper;
        private readonly ImageConfig _imageConfig;
        private readonly ILogger<PhotoService> _logger;

        public PhotoService(IFileService fileService,
            IRepository<Photo> photoRepository,
            IMapper mapper,
            IOptions<ImageConfig> imageOptions,
            ILogger<PhotoService> logger)
        {
            _fileService = fileService;
            _photoRepository = photoRepository;
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

        public async Task<PhotoResponseModel> GetByIdAsync(int id)
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

        public async Task<IEnumerable<PhotoResponseModel>> GetByIdsAsync(IEnumerable<int> ids)
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

        public async Task<IEnumerable<PhotoResponseModel>> GetByAlbumIdAsync(int albumId)
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

        public async Task<PhotoResponseModel> SavePhotoAsync(Album album, byte[] bytes, string origFileName, string contentType, bool resize = true)
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
            var rootPath = ResolveFilePath(albumId);
            var extension = Path.GetExtension(origFileName);

            var saveFileName = $"{Guid.NewGuid()}{extension}";

            _logger.LogDebug($"Saving photo to album ID {albumId}");

            // Save the original [renamed] file
            using (var stream = new MemoryStream(imageBytes))
            {
                var fileName = $"{rootPath}/{saveFileName}";
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
                            var fileName = GetResizedFileName(saveFileName, sizeProfile.Key);
                            var filePath = $"{rootPath}/{fileName}";

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
            var originalFilePath = ResolveFilePath(photo.AlbumId, photo.Filename);

            await _fileService.DeleteFileAsync(originalFilePath);
            _logger.LogDebug($"Deleted photo file from storage: {originalFilePath} (ID: {photo.Id})");

            foreach(var size in _imageConfig.SizeProfiles)
            {
                var sizeFilePath = ResolveFilePath(photo.AlbumId, GetResizedFileName(photo.Filename, size.Key));
                await _fileService.DeleteFileAsync(sizeFilePath);

                _logger.LogDebug($"Deleted photo file from storage: {originalFilePath} (ID: {photo.Id})");
            }
        }

        private string ResolveFilePath(int albumId, string fileName = null)
        {
            var path = _imageConfig.BasePath.Replace("\\", "/").TrimEnd('/');
            path = $"{path}/albums/{albumId}";

            if (!string.IsNullOrEmpty(fileName))
            {
                path = $"{path}/{fileName}";
            }

            return path;
        }

        private PhotoResponseModel ToModel(Photo photo)
        {
            // TODO: Replace with Automapper

            var model = _mapper.Map<PhotoResponseModel>(photo);

            model.Files.Add("original", _fileService.ResolveFileUrl(ResolveFilePath(photo.AlbumId, photo.Filename)));
            foreach (var size in _imageConfig.SizeProfiles)
            {
                var sizeFileName = GetResizedFileName(photo.Filename, size.Key);
                var path = ResolveFilePath(photo.AlbumId, sizeFileName);

                var url = _fileService.ResolveFileUrl(path);
                model.Files.Add(size.Key, url);
            }

            return model;
        }

        private static string GetResizedFileName(string fileName, string sizeKey) => $"{sizeKey}_{fileName}";
    }
}
