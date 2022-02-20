using System;
using System.Threading.Tasks;
using CleanArchitecture.Shopping.Core.BankAccount;
using CleanArchitecture.Shopping.Core.Bill;
using CleanArchitecture.Shopping.Core.User;
using CleanArchitecture.Shopping.Infrastructure.Database.Budget;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shopping.Infrastructure.Database
{
    public static class DatabaseSeed
    {
        public static readonly IdentityUser TestUser1 = new("user@email.at")
        {
            Email = "user@email.at",
        };

        public static readonly IdentityUser TestUser2 = new("user2@email.at")
        {
            Email = "user2@email.at"
        };

        private static readonly User TestUser1Sql = new(new Guid(TestUser1.Id), TestUser1.UserName);
        private static readonly User TestUser2Sql = new(new Guid(TestUser2.Id), TestUser2.UserName);

        public static async Task SeedAsync(
            ShoppingDbContext context,
            UserManager<IdentityUser> userManager)
        {
            await InsertUserAsync(context, userManager);
            await InsertAccountAsync(context);
            await InsertBillsAsync(context);
            await context.SaveChangesAsync();
        }

        private static async Task InsertUserAsync(
            ShoppingDbContext context,
            UserManager<IdentityUser> userManager)
        {
            var user1 = await userManager.FindByNameAsync("user@email.at");
            if (user1 == null)
            {
                await userManager.CreateAsync(TestUser1, "password");
            }

            var user2 = await userManager.FindByNameAsync("user2@email.at");
            if (user2 == null)
            {
                await userManager.CreateAsync(TestUser2, "password");
            }

            if (!await context.User.AnyAsync())
            {
                context.User.Add(TestUser1Sql);
                context.User.Add(TestUser2Sql);

                await context.SaveChangesAsync();
            }
        }

        private static async Task InsertAccountAsync(ShoppingDbContext context)
        {
            if (!await context.BankAccount.AnyAsync())
            {
                context.BankAccount.Add(new BankAccount("account1", new Guid(TestUser1.Id)));

                await context.SaveChangesAsync();
            }
        }

        private static async Task InsertBillsAsync(ShoppingDbContext context)
        {
            if (!await context.Bill.AnyAsync())
            {
                var account = await context.BankAccount.FirstOrDefaultAsync(a => a.Name == "account1");

                context.Bill.Add(new Bill(
                    TestUser1Sql,
                    account,
                    "ShopName1",
                    4.44,
                    DateTime.UtcNow.AddDays(-7),
                    "Notes1",
                    Category.Car));

                context.Bill.Add(new Bill(
                    TestUser1Sql,
                    account,
                    "ShopName2",
                    1.00,
                    DateTime.UtcNow.AddDays(-4),
                    "Notes2",
                    Category.Travelling));

                context.Bill.Add(new Bill(
                    TestUser1Sql,
                    account,
                    "ShopName3",
                    10.74,
                    DateTime.UtcNow.AddDays(3),
                    "Notes3",
                    Category.Sport));

                context.Bill.Add(new Bill(
                    TestUser1Sql,
                    account,
                    "ShopName4",
                    8.789,
                    DateTime.UtcNow.AddDays(7),
                    "Notes4",
                    Category.Gift));

                await context.SaveChangesAsync();
            }
        }
    }
}
