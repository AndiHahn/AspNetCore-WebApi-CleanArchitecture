using System.Text;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Configurations;
using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Database.Identity;
using CleanArchitecture.Infrastructure.Services.AzureStorage;
using CleanArchitecture.Infrastructure.Services.Email;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Web.Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.RegisterInfrastructure(configuration);
            services.RegisterCore();

            services.ConfigureAppSettings(configuration);

            return services;
        }

        private static void ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthenticationConfiguration>(configuration.GetSection("Authentication"));
            services.Configure<AzureStorageConfiguration>(configuration.GetSection("AzureStorage"));
            services.Configure<SendGridConfiguration>(configuration.GetSection("SendGrid"));
        }

        public static void AddAuthenticationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<IdentityContext>();

            var authConfig = configuration.GetSection("Authentication").Get<AuthenticationConfiguration>();
            byte[] key = Encoding.ASCII.GetBytes(authConfig.Secret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.Audience = "cleanarchitecture-api";
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = true
                    };
                });
        }

        /// <summary>
        /// Configures the ProblemDetails for Hellang.Middleware.ProblemDetails
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddProblemDetails(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddProblemDetails(opts =>
            {
                opts.Map<BadRequestException>(ex =>
                    new Microsoft.AspNetCore.Mvc.ProblemDetails
                    {
                        Title = "Bad Request",
                        Detail = ex.Message,
                        Status = 400,
                        Type = "https://httpstatuses.com/400"
                    });

                opts.Map<ForbiddenException>(ex =>
                    new Microsoft.AspNetCore.Mvc.ProblemDetails
                    {
                        Title = "Forbidden",
                        Detail = ex.Message,
                        Status = 403,
                        Type = "https://httpstatuses.com/403"
                    });

                opts.Map<NotFoundException>(ex =>
                    new Microsoft.AspNetCore.Mvc.ProblemDetails
                    {
                        Title = "Not Found",
                        Detail = ex.Message,
                        Status = 404,
                        Type = "https://httpstatuses.com/404"
                    });

                opts.Map<ConflictException>(ex =>
                    new Microsoft.AspNetCore.Mvc.ProblemDetails
                    {
                        Title = "Conflict",
                        Detail = ex.Message,
                        Status = 409,
                        Type = "https://httpstatuses.com/409"
                    });

                opts.IncludeExceptionDetails = (ctx, ex) =>
                {
                    var env = ctx.RequestServices.GetRequiredService<IHostEnvironment>();
                    string includeStackTraceConfig = configuration.GetSection("Logging")
                                                                  .GetSection("ProblemDetails")
                                                                  .GetSection("ForceIncludeStackTrace").Value;
                    if (bool.TryParse(includeStackTraceConfig, out bool includeStackTrace))
                    {
                        return env.IsDevelopment() || includeStackTrace;
                    }

                    return env.IsDevelopment();
                };
            });

            return services;
        }
    }
}
