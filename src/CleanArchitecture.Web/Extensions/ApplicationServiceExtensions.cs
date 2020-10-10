﻿using System.Text;
using CleanArchitecture.Core;
using CleanArchitecture.Core.Configurations;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Services;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Web.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("ApplicationDbConnection");
            services.AddDbContext<IBudgetContext, BudgetContext>(options => options.UseSqlServer(connectionString));

            services.RegisterServices();
            services.RegisterInfrastructure();
            services.RegisterCore();

            services.ConfigureAppSettings(configuration);

            return services;
        }

        public static void ConfigureAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthenticationConfiguration>(configuration.GetSection("Authentication"));
            services.Configure<BlobStorageConfiguration>(configuration.GetSection("BlobStorage"));
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
                {
                    return new Microsoft.AspNetCore.Mvc.ProblemDetails()
                    {
                        Title = "Bad Request",
                        Detail = ex.Message,
                        Status = 400,
                        Type = "https://httpstatuses.com/400",
                    };
                });

                opts.Map<NotFoundException>(ex =>
                {
                    var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails()
                    {
                        Title = "Not Found",
                        Detail = ex.Message,
                        Status = 404,
                        Type = "https://httpstatuses.com/404"
                    };

                    problemDetails.Extensions.Add("ErrorNumber", 10);

                    return problemDetails;
                });

                opts.Map<ConflictException>(ex =>
                {
                    return new Microsoft.AspNetCore.Mvc.ProblemDetails()
                    {
                        Title = "Conflict",
                        Detail = ex.Message,
                        Status = 409,
                        Type = "https://httpstatuses.com/409"
                    };
                });

                opts.Map<ProgrammingException>(ex =>
                {
                    return new Microsoft.AspNetCore.Mvc.ProblemDetails()
                    {
                        Title = "Programming Error",
                        Detail = ex.Message,
                        Status = 500,
                        Type = "https://httpstatuses.com/500"
                    };
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