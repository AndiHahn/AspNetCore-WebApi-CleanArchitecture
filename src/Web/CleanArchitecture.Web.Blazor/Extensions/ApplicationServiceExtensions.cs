using CleanArchitecture.Application;
using CleanArchitecture.Application.User;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Database.Identity;
using CleanArchitecture.Infrastructure.Services.AzureStorage;
using CleanArchitecture.Infrastructure.Services.Email;
using CleanArchitecture.Web.Blazor.Modules.Bill.Facades;
using CleanArchitecture.Web.Blazor.Modules.Core.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Web.Blazor.Extensions
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
            services.AddScoped<IBillFacade, BillFacade>();
            services.AddTransient<ICurrentUserService, CurrentUserService>();

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
                .AddDefaultUI()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

            services
                .AddAuthentication(AzureADB2CDefaults.AuthenticationScheme)
                .AddAzureADB2C(options => configuration.Bind("Authentication:AzureAdB2C", options))
                .AddGoogle(options => configuration.Bind("Authentication:Google", options))
                .AddFacebook(options => configuration.Bind("Authentication:Facebook", options));
        }
    }
}
