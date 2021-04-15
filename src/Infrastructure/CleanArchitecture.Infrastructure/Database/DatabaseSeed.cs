using System;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Data;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Database
{
    public static class DatabaseSeed
    {
        private static readonly IdentityUser TestUser1 = new IdentityUser("username")
        {
            Email = "user@email.at"
        };

        private static readonly IdentityUser TestUser2 = new IdentityUser("username2")
        {
            Email = "user2@email.at"
        };

        public static async Task SeedAsync(
            IBudgetContext context,
            UserManager<IdentityUser> userManager)
        {
            await InsertUserAsync(context, userManager);
            await InsertAccountAsync(context);
            await InsertUserAccountAsync(context);
            await InsertBillsAsync(context);
            await context.SaveChangesAsync();
        }

        private static async Task InsertUserAsync(
            IBudgetContext context,
            UserManager<IdentityUser> userManager)
        {
            if (!await userManager.Users.AnyAsync())
            {
                await userManager.CreateAsync(TestUser1, "password");
                await userManager.CreateAsync(TestUser2, "password2");
            }

            if (!await context.User.AnyAsync())
            {
                context.User.Add(new UserEntity(new Guid(TestUser1.Id)));
                context.User.Add(new UserEntity(new Guid(TestUser2.Id)));

                await context.SaveChangesAsync();
            }
        }

        private static async Task InsertAccountAsync(IBudgetContext context)
        {
            if (!await context.BankAccount.AnyAsync())
            {
                context.BankAccount.Add(new BankAccountEntity
                {
                    Name = "account1",
                    OwnerId = new Guid(TestUser1.Id)
                });

                await context.SaveChangesAsync();
            }
        }

        private static async Task InsertUserAccountAsync(IBudgetContext context)
        {
            if (!await context.UserBankAccount.AnyAsync())
            {
                var account = await context.BankAccount.FirstOrDefaultAsync(a => a.Name == "account1");

                if (account != null)
                {
                    context.UserBankAccount.Add(new UserBankAccountEntity(account.Id, new Guid(TestUser1.Id)));
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task InsertBillsAsync(IBudgetContext context)
        {
            if (!await context.Bill.AnyAsync())
            {
                var account = await context.BankAccount.FirstOrDefaultAsync(a => a.Name == "account1");

                context.Bill.Add(new BillEntity
                {
                    ShopName = "ShopName1",
                    Date = DateTime.UtcNow.AddDays(-7),
                    Notes = "Notes1",
                    Price = 4.44,
                    CreatedByUserId = new Guid(TestUser1.Id),
                    BankAccountId = account.Id,
                    Category = Domain.Enums.Category.Car
                });

                context.Bill.Add(new BillEntity
                {
                    ShopName = "ShopName2",
                    Date = DateTime.UtcNow.AddDays(-4),
                    Notes = "Notes2",
                    Price = 1.00,
                    CreatedByUserId = new Guid(TestUser1.Id),
                    BankAccountId = account.Id,
                    Category = Domain.Enums.Category.Travelling
                });

                context.Bill.Add(new BillEntity
                {
                    ShopName = "ShopName3",
                    Date = DateTime.UtcNow.AddDays(3),
                    Notes = "Notes3",
                    Price = 10.7,
                    CreatedByUserId = new Guid(TestUser1.Id),
                    BankAccountId = account.Id,
                    Category = Domain.Enums.Category.Sport
                });

                context.Bill.Add(new BillEntity
                {
                    ShopName = "ShopName4",
                    Date = DateTime.UtcNow.AddDays(7),
                    Notes = "Notes4",
                    Price = 8.4335,
                    CreatedByUserId = new Guid(TestUser1.Id),
                    BankAccountId = account.Id,
                    Category = Domain.Enums.Category.Gift
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
