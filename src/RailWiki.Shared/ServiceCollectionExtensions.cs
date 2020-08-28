using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RailWiki.Shared.Configuration;
using RailWiki.Shared.Data;
using RailWiki.Shared.Services.Users;

namespace RailWiki.Shared
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OktaConfig>(configuration.GetSection("Okta"));

            services.AddTransient<IUserService, UserService>();
        }

        public static void AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDbContext, RailWikiDbContext>();

            services.AddDbContext<RailWikiDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("RailWikiDb")));

            services.AddTransient(typeof(IRepository<>), typeof(EfRepository<>));
        }
        
    }
}
