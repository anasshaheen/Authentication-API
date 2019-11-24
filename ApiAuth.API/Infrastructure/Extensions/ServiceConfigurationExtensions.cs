using ApiAuth.API.Data;
using ApiAuth.API.Infrastructure.Abstraction.IServices;
using ApiAuth.API.Infrastructure.Options;
using ApiAuth.API.Infrastructure.Services;
using ApiAuth.API.Models;
using AutoMapper;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApiAuth.API.Infrastructure.Extensions
{
    public static class ServiceConfigurationExtensions
    {
        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtIssuerOptions>(options =>
            {
                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtIssuerOptions:Key"]));
                options.Issuer = configuration["JwtIssuerOptions:Issuer"];
                options.Audience = configuration["JwtIssuerOptions:Audience"];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.Configure<FacebookAuthSettings>(configuration.GetSection(nameof(FacebookAuthSettings)));
            services.Configure<GoogleAuthSettings>(configuration.GetSection(nameof(GoogleAuthSettings)));
            services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));

            return services;
        }

        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opts =>
                {
                    var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtIssuerOptions:Key"]));

                    opts.RequireHttpsMetadata = false;
                    opts.SaveToken = true;
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = signingKey,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };

                    opts.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/hub")))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }

        public static IServiceCollection AddIdentityConfig(this IServiceCollection services)
        {
            services.AddDefaultIdentity<AppUser>()
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<DataContext>();

            return services;
        }

        public static IServiceCollection AddDataContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IEmailTemplateGenerator, EmailTemplateGenerator>();
            services.AddScoped<IEmailDispatcher, EmailDispatcher>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>()
                    .ActionContext;
                return new UrlHelper(actionContext);
            });

            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            return services;
        }

        public static IServiceCollection AddComponents(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddAutoMapper(typeof(Startup).GetTypeInfo().Assembly);

            services.AddHttpClient();

            services.AddCors();

            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new Info { Title = "GP Taxi API", Version = "v1" });
            });

            services.AddMvc()
                .AddJsonOptions(opts =>
                {
                    opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSignalR();

            return services;
        }

        public static IServiceCollection AddHangFireConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(cfg => cfg.UseRedisStorage(configuration.GetConnectionString("RedisConnection")));
            services.AddHangfireServer();

            return services;
        }
    }
}
