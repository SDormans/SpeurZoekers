using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Speurzoekers.Business;
using Speurzoekers.Common.Business;
using Speurzoekers.Common.Config;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Data;
using Speurzoekers.Data.Entities.Identity;
using Speurzoekers.Data.Entities.Mappers;
using Speurzoekers.Data.Repositories;
using Speurzoekers.Service.Dto.Mappers;
using Speurzoekers.Service.Hubs;
using Speurzoekers.Service.Infrastructure;
using Speurzoekers.Service.Services;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Speurzoekers.Service
{
    public class Startup
    {
        private const string CorsPolicyName = "_allowAll";

        private readonly IConfigurationRoot _config;
        private readonly IConfigurationSection _authConfig, _domainConfig, _blobConfig;

        public Startup(IHostingEnvironment env)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
#if DEBUG
                .AddJsonFile("appsettings.Debug.json")
#else
                .AddJsonFile("appsettings.Release.json")
#endif
                .AddEnvironmentVariables();

            _config = builder.Build();
            _authConfig = _config.GetSection(ConfigurationKeys.AuthenticationSection);
            _domainConfig = _config.GetSection(ConfigurationKeys.DomainOptionsSection);
            _blobConfig = _config.GetSection(ConfigurationKeys.BlobStorageSection);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            AuthenticationOptions authOptions = _authConfig.Get<AuthenticationOptions>();

            // Options
            services.Configure<AuthenticationOptions>(_authConfig);
            services.Configure<DomainOptions>(_domainConfig);
            services.Configure<BlobStorage>(_blobConfig);

            // Mappers
            services.AddAutoMapper(
                typeof(UserDtoMapper), 
                typeof(CollectibleDtoMapper),
                typeof(AvatarDtoMapper),
                typeof(UserEntityMapper),
                typeof(RegionEntityMapper),
                typeof(CollectibleEntityMapper),
                typeof(AvatarEntityMapper));

            // SQL DB
            services.AddDbContext<SpeurzoekersDbContext>(options =>
                options.UseSqlServer(GetConnectionString())
            );
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICollectibleRepository, CollectibleRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IUserCoordinatesRepository, UserCoordinatesRepository>();
            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
            services.AddScoped<IAvatarRepository, AvatarRepository>();
            services.AddScoped<IActivityRepository, ActivityRepository>();

            // Blob storage
            services.AddScoped<IBlobRepository, BlobRepository>();

            // Identity
            byte[] key = Encoding.ASCII.GetBytes(authOptions.Secret);

            services.AddIdentity<ApplicationUser, ApplicationRole>(config=>
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<SpeurzoekersDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
#warning Check JWT token config
                //x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                x.Events = new JwtBearerEvents /* Pass token to SignalR hubs */
                {
                    OnMessageReceived = context =>
                    {
                        StringValues token = context.Request.Query["access_token"];

                        if (!string.IsNullOrEmpty(token)
                            && context.HttpContext.Request.Path.StartsWithSegments("/hubs"))
                        {
                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
            });

            int tokenMins = authOptions.MailTokenExpirationMinutes;
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromMinutes(tokenMins);
            });

            // Business
            services.AddScoped<IMapGenerator, MapGenerator>();

            // Services
            services.AddHostedService<CollectibleService>();
            services.AddScoped<IEmailService, EmailService>();            

            // Others
            services.AddMvc(o =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCors(options => options.AddPolicy(CorsPolicyName, builder => builder
                                        .SetIsOriginAllowed(_ => true) // this,
                                        //.AllowAnyOrigin()            // instead of,
                                        .AllowCredentials()            // or this wont work
                                        .AllowAnyMethod()
                                        .AllowAnyHeader()));
            services.AddSwaggerDocumentation();
            services.AddSignalR();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseSwaggerDocumentation();
            app.UseHttpsRedirection();
            app.UseCors(CorsPolicyName);
            app.UseMvc();

            app.UseSignalR(routes =>
            {
                routes.MapHub<CollectibleHub>("/hubs/CollectibleHub");
            });
        }

        private string GetConnectionString()
        {
            string con = null;

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    con = _config.GetValue<string>(ConfigurationKeys.DbConnectionStringWin);
                    break;
                case PlatformID.Unix:
                    con = _config.GetValue<string>(ConfigurationKeys.DbConnectionStringUnix);
                    break;                    
            }

            if (con != null)
            {
                return con;
            }
            else
            {
                string platform = Environment.OSVersion.Platform.ToString();
#if DEBUG
                string isDev = "Debug";
#else
                string isDev = "Release";
#endif
                throw new Exception($"No connection string defined for platform {platform} in {isDev} configuration.");
            }
        }
    }
}
