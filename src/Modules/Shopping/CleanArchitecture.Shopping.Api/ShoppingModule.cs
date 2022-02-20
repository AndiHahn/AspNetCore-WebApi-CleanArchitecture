using CleanArchitecture.Shopping.Application;
using CleanArchitecture.Shopping.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using CleanArchitecture.Shopping.Infrastructure.Database;
using CleanArchitecture.Shopping.Infrastructure.Database.Budget;
using CleanArchitecture.Shopping.Infrastructure.Database.Identity;

namespace CleanArchitecture.Shopping.Api
{
    public static class ShoppingModule
    {
        public static void AddShoppingModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddApplication();
            services.AddInfrastructure(configuration);
        }

        public static IMvcBuilder AddShoppingModule(this IMvcBuilder builder)
        {
            return builder.AddApplicationPart(Assembly.GetExecutingAssembly());
        }
        
        public static async Task MigrateAndSeedShoppingDbContextAsync(IServiceProvider services)
        {
            var shoppingDbContext = services.GetRequiredService<ShoppingDbContext>();
            await shoppingDbContext.Database.MigrateAsync();
            var identityDbContext = services.GetRequiredService<IdentityContext>();
            await identityDbContext.Database.MigrateAsync();

            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            await DatabaseSeed.SeedAsync(shoppingDbContext, userManager);
        }
    }
}
