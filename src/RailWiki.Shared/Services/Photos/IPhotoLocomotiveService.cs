using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Photos;

namespace RailWiki.Shared.Services.Photos
{
    public interface IPhotoLocomotiveService
    {
        Task<IEnumerable<int>> GetPhotoIdsForLocomotives(int locomotiveId);
    }

    public class PhotoLocomotiveService : IPhotoLocomotiveService
    {
        private readonly IRepository<PhotoLocomotive> _repository;

        public PhotoLocomotiveService(IRepository<PhotoLocomotive> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<int>> GetPhotoIdsForLocomotives(int locomotiveId)
        {
            var photoIds = await _repository.TableNoTracking
                .Where(x => x.LocomotiveId == locomotiveId)
                .Select(x => x.PhotoId)
                .ToListAsync();

            return photoIds;
        }
    }
}
