using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Models.Domain.User;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Database.Budget;
using CleanArchitecture.Infrastructure.Database.Identity;
using CleanArchitecture.Web.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CleanArchitecture.FunctionalTests.Fixture
{
    public class ApiFunctionalTestFixture : WebApplicationFactory<Startup>
    {
        public Guid UserId { get; private set; }
        private ServiceProvider serviceProvider;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var budgetContextDescriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<BudgetContext>));

                if (budgetContextDescriptor != null)
                {
                    services.Remove(budgetContextDescriptor);
                }

                var budgetIdentityContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<IdentityContext>));

                if (budgetIdentityContextDescriptor != null)
                {
                    services.Remove(budgetIdentityContextDescriptor);
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

                services.AddDbContext<IdentityContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                //Build the service provider.
                serviceProvider = services.BuildServiceProvider();

                //Create a scope to obtain a reference to the database context
                using var scope = serviceProvider.CreateScope();
                var scopedServices = scope.ServiceProvider;

                //Add test user for authentication

                var userManager = scopedServices.GetRequiredService<UserManager<IdentityUser>>();
                IdentityUser testUser = new IdentityUser("user@email.at")
                {
                    Email = "user@email.at"
                };

                Task.Run(async () => await userManager.CreateAsync(testUser, "password")).Wait();

                var context = scopedServices.GetRequiredService<BudgetContext>();
                UserId = new Guid(testUser.Id);

                context.User.Add(new UserEntity
                {
                    Id = new Guid(testUser.Id)
                });

                context.SaveChanges();

                //Ensure the database is created.
                context.Database.EnsureCreated();
            });
        }

        public async Task<HttpClient> CreateAuthorizedClientAsync()
        {
            var client = base.CreateClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/user/authenticate");

            var signInModel = new SignInModel
            {
                Username = "user@email.at",
                Password = "password"
            };

            string json = JsonConvert.SerializeObject(signInModel);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request);

            var authResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(
                await response.Content.ReadAsStringAsync());

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authResponse.Token);
            return client;
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
            context.UserBankAccount.RemoveRange(context.UserBankAccount);
            context.BankAccount.RemoveRange(context.BankAccount);
            context.User.RemoveRange(context.User);
        }
    }
}
