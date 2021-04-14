using System;
using System.Threading.Tasks;
using CleanArchitecture.Application;
using CleanArchitecture.Core.Interfaces.Data;
using CleanArchitecture.Core.Models.Common;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data
{
    public static class BudgetContextSeed
    {
        public static async Task SeedAsync(IBudgetContext context)
        {
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
                var password = new HashedPassword();
                password.WithPlainPasswordAndSaltSize("password", Constants.Authentication.SALT_SIZE);

                context.User.Add(new UserEntity()
                {
                    FirstName = "FirstName",
                    LastName = "LastName",
                    UserName = "username",
                    Password = password.Hash,
                    Salt = password.Salt
                });

                var password2 = new HashedPassword();
                password2.WithPlainPasswordAndSaltSize("password2", Constants.Authentication.SALT_SIZE);

                context.User.Add(new UserEntity
                {
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    UserName = "username2",
                    Password = password2.Hash,
                    Salt = password2.Salt
                });

                await context.SaveChangesAsync();
            }
        }

        private static async Task InsertAccountAsync(IBudgetContext context)
        {
            if (!await context.BankAccount.AnyAsync())
            {
                var user = await context.User.FirstOrDefaultAsync(u => u.UserName == "username");

                context.BankAccount.Add(new BankAccountEntity
                {
                    Name = "account1",
                    OwnerId = user.Id
                });

                await context.SaveChangesAsync();
            }
        }

        private static async Task InsertUserAccountAsync(IBudgetContext context)
        {
            if (!await context.UserBankAccount.AnyAsync())
            {
                var user = await context.User.FirstOrDefaultAsync(u => u.UserName == "username");
                var account = await context.BankAccount.FirstOrDefaultAsync(a => a.Name == "account1");

                if (user != null && account != null)
                {
                    context.UserBankAccount.Add(new UserBankAccountEntity(account.Id, user.Id));
                }

                await context.SaveChangesAsync();
            }
        }

        private static async Task InsertBillsAsync(IBudgetContext context)
        {
            if (!await context.Bill.AnyAsync())
            {
                var user = await context.User.FirstOrDefaultAsync(u => u.UserName == "username");
                var account = await context.BankAccount.FirstOrDefaultAsync(a => a.Name == "account1");

                context.Bill.Add(new BillEntity
                {
                    ShopName = "ShopName1",
                    Date = DateTime.UtcNow.AddDays(-7),
                    Notes = "Notes1",
                    Price = 4.44,
                    CreatedByUserId = user.Id,
                    BankAccountId = account.Id,
                    Category = Domain.Enums.Category.Car
                });

                context.Bill.Add(new BillEntity
                {
                    ShopName = "ShopName2",
                    Date = DateTime.UtcNow.AddDays(-4),
                    Notes = "Notes2",
                    Price = 1.00,
                    CreatedByUserId = user.Id,
                    BankAccountId = account.Id,
                    Category = Domain.Enums.Category.Travelling
                });

                context.Bill.Add(new BillEntity
                {
                    ShopName = "ShopName3",
                    Date = DateTime.UtcNow.AddDays(3),
                    Notes = "Notes3",
                    Price = 10.7,
                    CreatedByUserId = user.Id,
                    BankAccountId = account.Id,
                    Category = Domain.Enums.Category.Sport
                });

                context.Bill.Add(new BillEntity
                {
                    ShopName = "ShopName4",
                    Date = DateTime.UtcNow.AddDays(7),
                    Notes = "Notes4",
                    Price = 8.4335,
                    CreatedByUserId = user.Id,
                    BankAccountId = account.Id,
                    Category = Domain.Enums.Category.Gift
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
