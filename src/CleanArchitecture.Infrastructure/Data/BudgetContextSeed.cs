using System;
using System.Threading.Tasks;
using CleanArchitecture.Core.Helper;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data
{
    public class BudgetContextSeed
    {
        public static async Task SeedAsync(IBudgetContext context)
        {
            await InsertBillCategoriesAsync(context);
            await InsertUserAsync(context);
            await InsertAccountAsync(context);
            await InsertUserAccountAsync(context);
            await InsertBillsAsync(context);
            await context.SaveChangesAsync();
        }

        private static async Task InsertUserAsync(IBudgetContext context)
        {
            if (!await context.User.AnyAsync())
            {
                context.User.Add(new UserEntity()
                {
                    FirstName = "FirstName",
                    LastName = "LastName",
                    UserName = "username",
                    Password = PasswordHelper.GenerateHash("password", "salt"),
                    Salt = "salt"
                });

                await context.SaveChangesAsync();
            }
        }

        private static async Task InsertAccountAsync(IBudgetContext context)
        {
            if (!await context.Account.AnyAsync())
            {
                context.Account.Add(new AccountEntity("MyAccount"));

                await context.SaveChangesAsync();
            }
        }

        private static async Task InsertUserAccountAsync(IBudgetContext context)
        {
            if (!await context.UserAccount.AnyAsync())
            {
                var user = await context.User.FirstOrDefaultAsync();
                var account = await context.Account.FirstOrDefaultAsync();

                if (user != null && account != null)
                {
                    context.UserAccount.Add(new UserAccountEntity(account.Id, user.Id));
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task InsertBillCategoriesAsync(IBudgetContext context)
        {
            if (!await context.BillCategory.AnyAsync())
            {
                context.BillCategory.Add(new BillCategoryEntity("Food"));
                context.BillCategory.Add(new BillCategoryEntity("Restaurant"));
                context.BillCategory.Add(new BillCategoryEntity("Flat"));
                context.BillCategory.Add(new BillCategoryEntity("Clothes"));

                await context.SaveChangesAsync();
            }
        }

        private static async Task InsertBillsAsync(IBudgetContext context)
        {
            if (!await context.Bill.AnyAsync())
            {
                var user = await context.User.FirstOrDefaultAsync();
                var account = await context.Account.FirstOrDefaultAsync();
                var categories = await context.BillCategory.ToListAsync();

                context.Bill.Add(new BillEntity()
                {
                    ShopName = "ShopName1",
                    Date = DateTime.UtcNow.AddDays(-7),
                    Notes = "Notes1",
                    Price = 4.44,
                    UserId = user.Id,
                    AccountId = account.Id,
                    BillCategoryId = categories[0].Id
                });

                context.Bill.Add(new BillEntity()
                {
                    ShopName = "ShopName2",
                    Date = DateTime.UtcNow.AddDays(-4),
                    Notes = "Notes2",
                    Price = 1.00,
                    UserId = user.Id,
                    AccountId = account.Id,
                    BillCategoryId = categories[1].Id
                });

                context.Bill.Add(new BillEntity()
                {
                    ShopName = "ShopName3",
                    Date = DateTime.UtcNow.AddDays(3),
                    Notes = "Notes3",
                    Price = 10.7,
                    UserId = user.Id,
                    AccountId = account.Id,
                    BillCategoryId = categories[2].Id
                });

                context.Bill.Add(new BillEntity()
                {
                    ShopName = "ShopName4",
                    Date = DateTime.UtcNow.AddDays(7),
                    Notes = "Notes4",
                    Price = 8.4335,
                    UserId = user.Id,
                    AccountId = account.Id,
                    BillCategoryId = categories[0].Id
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
