using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Roster;

namespace RailWiki.Shared.Services.Roster
{
    public interface IRoadService
    {
        Task<Road> GetById(int id);
        Task<Road> GetByReportingMarksAsync(string reportMarks);
    }

    public class RoadService : IRoadService
    {
        private readonly IRepository<Road> _roadRepository;

        public RoadService(IRepository<Road> roadRepository)
        {
            _roadRepository = roadRepository;
        }

        public async Task<Road> GetById(int id) =>
            await _roadRepository.GetByIdAsync(id);

        public async Task<Road> GetByReportingMarksAsync(string reportMarks) =>
            await _roadRepository.TableNoTracking.FirstOrDefaultAsync(x => x.ReportingMarks.ToUpper() == reportMarks.ToUpper());
    }
}
