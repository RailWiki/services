using System.Threading.Tasks;
using RailWiki.Shared.Entities.Roster;

namespace RailWiki.Shared.Services.Roster
{
    public interface ILocomotiveService
    {
        Task<Locomotive> FindByRoadNumberAsync(string roadNumber);

        Task CreateAsync(Locomotive locomotive);
    }
}
