using System;
using System.Collections.Generic;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Tests.Shared.Builder.Bill
{
    public class BillEntityBuilder
    {
        private readonly BillEntity bill;

        public BillEntityBuilder()
        {
            bill = new BillEntity
            {
                ShopName = "ShopName",
                Price = 10.21,
                Notes = "Notes",
                Date = DateTime.UtcNow,
                Category = Domain.Enums.Category.Travelling,
                UserBills = new List<UserBillEntity>()
            };
        }

        public BillEntityBuilder WithAccount(BankAccountEntity bankAccount)
        {
            bill.BankAccountId = bankAccount.Id;
            bill.BankAccount = bankAccount;
            return this;
        }

        public BillEntityBuilder CreatedByUser(UserEntity user)
        {
            bill.CreatedByUserId = user.Id;
            bill.UserBills.Add(new UserBillEntity
            {
                Bill = bill,
                BillId = bill.Id,
                User = user,
                UserId = user.Id
            });

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
