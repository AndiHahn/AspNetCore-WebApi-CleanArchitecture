using System;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Tests.Shared.Builder.Bill
{
    public class BillEntityBuilder
    {
        private readonly BillEntity bill;

        public BillEntityBuilder()
        {
            var billCategory = new BillCategoryEntity()
            {
                Name = "Category",
                Color = "red"
            };

            bill = new BillEntity()
            {
                ShopName = "ShopName",
                Price = 10.21,
                Notes = "Notes",
                Date = DateTime.UtcNow,
                PictureURL = "PictureUrl",
                BillCategory = billCategory
            };
        }

        public BillEntityBuilder WithAccount(AccountEntity account)
        {
            bill.AccountId = account.Id;
            bill.Account = account;
            return this;
        }

        public BillEntityBuilder WithUser(UserEntity user)
        {
            bill.UserId = user.Id;
            bill.User = user;
            return this;
        }

        public BillEntityBuilder WithDate(DateTime date)
        {
            bill.Date = date;
            return this;
        }

        public BillEntity Build()
        {
            return bill;
        }
    }
}
