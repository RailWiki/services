using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RailWiki.Shared.Data;

namespace RailWiki.Shared
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDbContext, RailWikiDbContext>();

            services.AddDbContext<RailWikiDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("RailWikiDb")));

            services.AddTransient(typeof(IRepository<>), typeof(EfRepository<>));
        }
        
    }
}
