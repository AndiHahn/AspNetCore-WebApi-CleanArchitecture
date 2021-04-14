using System.Text;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Configurations;
using CleanArchitecture.Core.Interfaces.Data;
using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Services.AzureStorage;
using CleanArchitecture.Infrastructure.Services.Email;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Web.Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("ApplicationDbConnection");
            services.AddDbContext<IBudgetContext, BudgetContext>(options => options.UseSqlServer(connectionString));

            services.RegisterInfrastructure();
            services.RegisterCore();

            services.ConfigureAppSettings(configuration);

            return services;
        }

        public static void ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthenticationConfiguration>(configuration.GetSection("Authentication"));
            services.Configure<AzureStorageConfiguration>(configuration.GetSection("AzureStorage"));
            services.Configure<SendGridConfiguration>(configuration.GetSection("SendGrid"));
        }

        public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authConfig = configuration.GetSection("Authentication").Get<AuthenticationConfiguration>();

            var key = Encoding.ASCII.GetBytes(authConfig.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }

        /// <summary>
        /// Configures the ProblemDetails for Hellang.Middleware.ProblemDetails
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureProblemDetails(this IServiceCollection services, IConfiguration configuration)
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
