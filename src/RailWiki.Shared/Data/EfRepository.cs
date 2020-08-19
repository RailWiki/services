using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RailWiki.Shared.Data
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private DbSet<TEntity> _entities;
        private readonly IDbContext _context;

        protected DbSet<TEntity> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Set<TEntity>();
                }

                return _entities;
            }
        }

        public IQueryable<TEntity> Table
        {
            get { return Entities; }
        }

        public IQueryable<TEntity> TableNoTracking
        {
            get { return Entities.AsNoTracking(); }
        }

        public EfRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            return await Entities.FindAsync(id);
        }

        public async Task CreateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                Entities.Add(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception(GetErrorText(dbEx), dbEx);
            }
        }

        public async Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                _context.Set<TEntity>().Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception(GetErrorText(dbEx), dbEx);
            }
        }

        public async Task DeleteAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                Entities.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception(GetErrorText(dbEx), dbEx);
            }
        }

        private string GetErrorText(DbUpdateException dbEx) => dbEx.Message;
    }
}
