using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Okta.AspNet.Abstractions;
using RailWiki.Shared;
using RailWiki.Shared.Data;
using RailWiki.Shared.Entities.Users;
using RailWiki.Shared.Services;

namespace RailWiki.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.WithOrigins(
                        "http://localhost:8080"
                    );
                });
            });

            services.AddDataAccess(Configuration);
            services.AddServices(Configuration);
            services.AddAmazonServices(Configuration);

            services.AddAutoMapper(typeof(Startup), typeof(ModelMapper));

            var oktaDomain = Configuration["Okta:OktaDomain"];

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var issuer = $"{oktaDomain}/oauth2/default";
                    var oktaOptions = new OktaWebApiOptions();

                    var tokenValidationParams = new DefaultTokenValidationParameters(oktaOptions, issuer)
                    {
                        ValidAudience = oktaOptions.Audience
                    };

                    options.Authority = issuer;
                    options.Audience = oktaOptions.Audience;
                    options.SecurityTokenValidators.Clear();
                    options.SecurityTokenValidators.Add(new StrictSecurityTokenValidator());

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            // TODO: Replace with service
                            var userEmail = context.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                            var userRepository = context.HttpContext.RequestServices.GetRequiredService<IRepository<User>>();
                            var user = await userRepository.TableNoTracking.FirstOrDefaultAsync(x => x.EmailAddress == userEmail);
                            
                            if (user != null)
                            {
                                var appClaims = new List<Claim>
                                {
                                    new Claim(AppClaimTypes.UserId, user.Id.ToString(CultureInfo.InvariantCulture), null, AppClaimTypes.AppClaimsIssuer),
                                };

                                var appIdentity = new ClaimsIdentity(appClaims);
                                context.Principal.AddIdentity(appIdentity);
                            }
                        }
                    };
                });

            services.AddAuthorization();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "RailWiki API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    OpenIdConnectUrl = new Uri($"{oktaDomain}/oauth2/default/v1/.well-known/openid-configuration"),
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{oktaDomain}/oauth2/default/v1/authorize"),
                            TokenUrl = new Uri($"{oktaDomain}/oauth2/default/v1/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "" },
                                { "profile", "" }
                            }
                        }
                    }
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                        },
                        new[] { "read" }
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseCors();

            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "RailWiki API v1");
                config.RoutePrefix = "api-docs";
                config.DocumentTitle = "RailWiki API";

                config.OAuthClientId(configuration["Okta:SwaggerClientId"]);
                config.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
                {
                    { "nonce", Guid.NewGuid().ToString().Replace("-", "") }
                });

                config.EnableDeepLinking();
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
