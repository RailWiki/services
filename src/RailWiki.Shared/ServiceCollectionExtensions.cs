using Amazon.S3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RailWiki.Shared.Configuration;
using RailWiki.Shared.Data;
using RailWiki.Shared.Security;
using RailWiki.Shared.Services;
using RailWiki.Shared.Services.FileStorage;
using RailWiki.Shared.Services.Photos;
using RailWiki.Shared.Services.Roster;
using RailWiki.Shared.Services.Users;

namespace RailWiki.Shared
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OktaConfig>(configuration.GetSection("Okta"));
            services.Configure<ImageConfig>(configuration.GetSection("Images"));

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy(Policies.AlbumOwnerOrMod, policy => policy.Requirements.Add(new OwnerOrModRequirement()));
                opts.AddPolicy(Policies.PhotoOwnerOrMod, policy => policy.Requirements.Add(new OwnerOrModRequirement()));
            });
            services.AddSingleton<IAuthorizationHandler, AlbumAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, PhotoAuthorizationHandler>();

            services.AddHttpClient();

            services.AddTransient<IRoadService, RoadService>();
            services.AddTransient<ILocomotiveService, LocomotiveService>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPhotoService, PhotoService>();
            services.AddTransient<IImportPhotoService, ImportPhotoService>();
            services.AddTransient<IPhotoLocomotiveService, PhotoLocomotiveService>();

            services.AddTransient<ICrossReferenceService, CrossReferenceService>();
        }

        public static void AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDbContext, RailWikiDbContext>();

            services.AddDbContext<RailWikiDbContext>(options => options
                .UseSqlServer(configuration.GetConnectionString("RailWikiDb")));

            services.AddTransient(typeof(IRepository<>), typeof(EfRepository<>));
        }

        public static void AddAmazonServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FileStorageConfig>(configuration.GetSection("FileStorage"));

            services.AddDefaultAWSOptions(configuration.GetAWSOptions());

            services.AddAWSService<IAmazonS3>();
            services.AddTransient<IFileService, AmazonS3FileService>();

            //services.AddAWSService<IAmazonSimpleEmailService>();
        }
    }
}
