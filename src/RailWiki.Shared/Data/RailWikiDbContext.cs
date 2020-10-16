using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RailWiki.Shared.Entities;
using RailWiki.Shared.Entities.Geography;
using RailWiki.Shared.Entities.Photos;
using RailWiki.Shared.Entities.Roster;
using RailWiki.Shared.Entities.Users;

namespace RailWiki.Shared.Data
{
    public class RailWikiDbContext : DbContext, IDbContext
    {
        public DbSet<Country> Countries { get; set; }
        public DbSet<StateProvince> StateProvinces { get; set; }
        public DbSet<Location> Locations { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<RoadType> RoadTypes { get; set; }
        public DbSet<Road> Roads { get; set; }
        public DbSet<RoadAlternateName> RoadAlternateNames { get; set; }
        public DbSet<Locomotive> Locomotives { get; set; }
        public DbSet<LocomotiveType> LocomotiveTypes { get; set; }
        public DbSet<RollingStockClass> RollingStockClasses { get; set; }
        public DbSet<RollingStockType> RollingStockTypes { get; set; }
        public DbSet<RollingStock> RollingStockItems { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<PhotoCategory> PhotoCategories { get; set; }
        public DbSet<PhotoLocomotive> PhotoLocomotives { get; set; }
        public DbSet<PhotoRollingStock> PhotoRollingStocks { get; set; }

        public DbSet<CrossReference> CrossReferences { get; set; }

        public new DatabaseFacade Database => base.Database;

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class, new() => base.Set<TEntity>();

        public RailWikiDbContext(DbContextOptions<RailWikiDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //var typeConfigurations = Assembly.GetExecutingAssembly().GetTypes()
            //    .Where(x => (x.BaseType?.IsGenericType ?? false)
            //        && x.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));

            //foreach (var config in typeConfigurations)
            //{
            //    var typeConfiguration = (IMappingConfiguration)Activator.CreateInstance(config);
            //    typeConfiguration.ApplyConfiguration(builder);
            //}

            //builder.AddIdentityConfigurations();
        }
    }
}
