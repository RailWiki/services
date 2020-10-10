using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RailWiki.Shared.Data
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        IQueryable<TEntity> Table { get; }
        IQueryable<TEntity> TableNoTracking { get; }

        Task<TEntity> GetByIdAsync(object id);

        Task CreateAsync(TEntity entity);
        Task CreateAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(IEnumerable<TEntity> entities);
    }
}
