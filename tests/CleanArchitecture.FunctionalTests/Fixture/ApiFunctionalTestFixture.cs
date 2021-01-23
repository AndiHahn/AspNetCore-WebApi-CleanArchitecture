using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using CleanArchitecture.Application;
using CleanArchitecture.Core.Models.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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
                    var testUser = CreateTestUserEntity();
                    UserId = testUser.Id;
                    context.User.Add(testUser);
                    context.SaveChanges();

                    //Ensure the database is created.
                    context.Database.EnsureCreated();
                }
            });
        }

        public HttpClient CreateAuthorizedClient()
        {
            var client = base.CreateClient();
            string token = CreateToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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

        private UserEntity CreateTestUserEntity()
        {
            var password = new HashedPassword();
            password.WithPlainPasswordAndSaltSize("password", Constants.Authentication.SALT_SIZE);

            return new UserEntity
            {
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                UserName = "Testuser",
                Password = password.Hash,
                Salt = password.Salt
            };
        }

        private string CreateToken()
        {
            string jwtSecret = "12345678901234567890123456789012";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
