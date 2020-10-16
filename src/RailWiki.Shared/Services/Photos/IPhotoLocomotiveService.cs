using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Photos;

namespace RailWiki.Shared.Services.Photos
{
    public interface IPhotoLocomotiveService
    {
        Task<IEnumerable<int>> GetPhotoIdsForLocomotives(int locomotiveId);
        Task<IEnumerable<PhotoLocomotive>> GetByPhotoId(int photoId);
        Task UpdatePhotoLocomotivesAsync(int photoId, List<int> locomotiveIds);
    }

    public class PhotoLocomotiveService : IPhotoLocomotiveService
    {
        private readonly IRepository<PhotoLocomotive> _repository;
        private readonly ILogger<PhotoLocomotiveService> _logger;

        public PhotoLocomotiveService(IRepository<PhotoLocomotive> repository,
            ILogger<PhotoLocomotiveService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<int>> GetPhotoIdsForLocomotives(int locomotiveId)
        {
            var photoIds = await _repository.TableNoTracking
                .Where(x => x.LocomotiveId == locomotiveId)
                .Select(x => x.PhotoId)
                .ToListAsync();

            return photoIds;
        }

        public async Task<IEnumerable<PhotoLocomotive>> GetByPhotoId(int photoId)
        {
            var locos = await _repository.Table
                .Include(x => x.Locomotive)
                .ThenInclude(x => x.Road)
                .Where(x => x.PhotoId == photoId)
                .ToListAsync();

            return locos;
        }

        public async Task UpdatePhotoLocomotivesAsync(int photoId, List<int> locomotiveIds)
        {
            // TODO: Perhaps we should use a model for this?

            try
            {
                // Get all locomotives currently assigned to the photo
                var photoLocos = await _repository.Table
                    .Where(x => x.PhotoId == photoId)
                    .ToListAsync();

                // Determine which locos need to be removed
                var locosToRemove = photoLocos.Where(x => !locomotiveIds.Contains(x.LocomotiveId));
                await _repository.DeleteAsync(locosToRemove);

                // Then determine which ones need to be added
                var locoIdsToAdd = locomotiveIds.Where(x => !photoLocos.Select(y => y.LocomotiveId).Contains(x));

                var locosToAdd = locoIdsToAdd.Select(x => new PhotoLocomotive
                {
                    PhotoId = photoId,
                    LocomotiveId = x
                });

                await _repository.CreateAsync(locosToAdd);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating photo locomotives");
                throw;
            }
        }
    }
}
