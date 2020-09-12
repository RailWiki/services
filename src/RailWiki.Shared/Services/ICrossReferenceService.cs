using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities;

namespace RailWiki.Shared.Services
{
    public interface ICrossReferenceService
    {
        Task<CrossReference> GetAsync(Type entityType, string source, string sourceId);

        Task<int?> GetEntityIdAsync(Type entityType, string source, string sourceId);

        Task CreateAsync(CrossReference crossReference);
        Task CreateAsync(Type entityType, int entityId, string source, string sourceId);
    }

    public class CrossReferenceService : ICrossReferenceService
    {
        private readonly IRepository<CrossReference> _repository;

        public CrossReferenceService(IRepository<CrossReference> repository)
        {
            _repository = repository;
        }

        public Task<CrossReference> GetAsync(Type entityType, string source, string sourceId) =>
            _repository.Table.FirstOrDefaultAsync(x => x.EntityType == entityType.Name && x.Source == source && x.SourceIdentifier == sourceId);

        public async Task<int?> GetEntityIdAsync(Type entityType, string source, string sourceId) =>
            (await GetAsync(entityType, source, sourceId))?.EntityId;

        public Task CreateAsync(CrossReference crossReference) =>
            _repository.CreateAsync(crossReference);

        public Task CreateAsync(Type entityType, int entityId, string source, string sourceId) =>
            CreateAsync(new CrossReference
            {
                EntityType = entityType.Name,
                EntityId = entityId,
                Source = source,
                SourceIdentifier = sourceId
            });
    }
}
