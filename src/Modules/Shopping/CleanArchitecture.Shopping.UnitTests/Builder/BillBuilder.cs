using System;
using CleanArchitecture.Shopping.Core.BankAccount;
using CleanArchitecture.Shopping.Core.Bill;
using CleanArchitecture.Shopping.Core.User;

namespace CleanArchitecture.Shopping.UnitTests.Builder
{
    public class BillBuilder
    {
        private User user;
        private BankAccount bankAccount;
        private string shopName = "ShopName";
        private double price = 7.45;
        private DateTime date = DateTime.UtcNow;
        private string notes = string.Empty;
        private Category category = Category.Car;

        public BillBuilder WithAccount(BankAccount bankAccount)
        {
            this.bankAccount = bankAccount;
            return this;
        }

        public BillBuilder CreatedByUser(User user)
        {
            this.user = user;
            return this;
        }

        public BillBuilder WithDate(DateTime date)
        {
            this.date = date;
            return this;
        }

        public Bill Build()
        {
            return new Bill(
                this.user,
                this.bankAccount,
                this.shopName,
                this.price,
                this.date,
                this.notes,
                this.category);
        }
    }
}
