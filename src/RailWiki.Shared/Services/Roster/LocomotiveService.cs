using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Roster;

namespace RailWiki.Shared.Services.Roster
{
    public class LocomotiveService : ILocomotiveService
    {
        private readonly IRepository<Locomotive> _locomotiveRepository;

        public LocomotiveService(IRepository<Locomotive> locomotiveRepository)
        {
            _locomotiveRepository = locomotiveRepository;
        }

        public async Task<Locomotive> FindByRoadNumberAsync(string roadNumber)
        {
            var loco = await _locomotiveRepository.TableNoTracking
                .FirstOrDefaultAsync(x => x.RoadNumber.ToUpper() == roadNumber.ToUpper());

            return loco;
        }

        public async Task CreateAsync(Locomotive locomotive) =>
            await _locomotiveRepository.CreateAsync(locomotive);
    }
}
