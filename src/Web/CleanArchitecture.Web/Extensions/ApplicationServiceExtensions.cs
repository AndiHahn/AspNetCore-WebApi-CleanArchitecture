using System.Text;
using CleanArchitecture.Application;
using CleanArchitecture.Application.User;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Database.Identity;
using CleanArchitecture.Infrastructure.Services.AzureStorage;
using CleanArchitecture.Infrastructure.Services.Email;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.RegisterApplicationCore();

            services.AddHttpContextAccessor();

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
                    x.Audience = "clean-architecture";
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = true
                    };
                });
        }
    }
}
