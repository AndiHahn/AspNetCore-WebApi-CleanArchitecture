﻿using CleanArchitecture.Shared.Infrastructure.Email;
using CleanArchitecture.Shopping.Application.User;
using CleanArchitecture.Shopping.Infrastructure.AzureStorage;
using CleanArchitecture.Shopping.Infrastructure.Database.Identity;
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
