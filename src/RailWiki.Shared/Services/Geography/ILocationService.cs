using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Geography;
using RailWiki.Shared.Models.Geography;

namespace RailWiki.Shared.Services.Geography
{
    public interface ILocationService
    {
        Task<IEnumerable<GetLocationModel>> SearchAsync(string name, int? stateProvinceId = null);
    }

    public class LocationService : ILocationService
    {
        private readonly IRepository<Location> _locationRepository;
        private readonly IMapper _mapper;

        public LocationService(IRepository<Location> locationRepository,
            IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetLocationModel>> SearchAsync(string name, int? stateProvinceId = null)
        {
            // TODO: Add pagination

            var locations = await _locationRepository.TableNoTracking
                .Where(x => (!stateProvinceId.HasValue || x.StateProvinceId == stateProvinceId.Value)
                    && (string.IsNullOrEmpty(name) || x.Name.Contains(name)))
                .OrderBy(x => x.Name)
                .ProjectTo<GetLocationModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return locations;
        }
    }
}
