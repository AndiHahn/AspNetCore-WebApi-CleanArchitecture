using System;
using System.Linq;
using CleanArchitecture.Application;
using CleanArchitecture.Core;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Models.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.FunctionalTests.Helper
{
    public class ApiTestFixture : WebApplicationFactory<Startup>
    {
        protected ServiceProvider serviceProvider;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            //var config = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.test.json")
            //    .Build();
            //builder.UseConfiguration(config);

            builder.ConfigureServices(services =>
            {
                var budgetContextDescriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<BudgetContext>));

                if (budgetContextDescriptor != null)
                {
                    services.Remove(budgetContextDescriptor);
                }

                //Create a new service provider.
                var provider = services
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                //Add a database context (ApplicationDbContext) using an in-memory 
                //database for testing.
                services.AddDbContext<BudgetContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                //Build the service provider.
                serviceProvider = services.BuildServiceProvider();

                //Create a scope to obtain a reference to the database context
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<BudgetContext>();

                    //Add test user for authentication
                    context.User.Add(GetTestUserEntity());
                    context.SaveChanges();

                    //Ensure the database is created.
                    context.Database.EnsureCreated();
                }
            });
        }

        public void SetupDatabase(Action<BudgetContext> setupCallback = null)
        {
            //Create a scope to obtain a reference to the database context
            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<BudgetContext>();

            ClearEntitiesInContext(context);

            setupCallback?.Invoke(context);

            context.SaveChanges();
        }

        private void ClearEntitiesInContext(BudgetContext context)
        {
            context.Bill.RemoveRange(context.Bill);
            context.BillCategory.RemoveRange(context.BillCategory);
            context.UserAccount.RemoveRange(context.UserAccount);
            context.Account.RemoveRange(context.Account);
            context.User.RemoveRange(context.User);
            context.Income.RemoveRange(context.Income);
            context.FixedCost.RemoveRange(context.FixedCost);
        }

        private UserEntity GetTestUserEntity()
        {
            var password = new HashedPassword();
            password.WithPlainPasswordAndSaltSize("password", Constants.Authentication.SALT_SIZE);

            return new UserEntity()
            {
                FirstName = "Test",
                LastName = "User",
                UserName = "Testuser",
                Password = password.Hash,
                Salt = password.Salt
            };
        }
    }
}
