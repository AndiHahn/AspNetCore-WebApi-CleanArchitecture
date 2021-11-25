using System;

namespace CleanArchitecture.Tests.Shared.Builder
{
    public class BillBuilder
    {
        private Core.User user;
        private Core.BankAccount bankAccount;
        private string shopName = "ShopName";
        private double price = 7.45;
        private DateTime date = DateTime.UtcNow;
        private string notes = string.Empty;
        private Core.Category category = Core.Category.Car;

        public BillBuilder WithAccount(Core.BankAccount bankAccount)
        {
            this.bankAccount = bankAccount;
            return this;
        }

        public BillBuilder CreatedByUser(Core.User user)
        {
            this.user = user;
            return this;
        }

        public BillBuilder WithDate(DateTime date)
        {
            this.date = date;
            return this;
        }

        public Core.Bill Build()
        {
            return new Core.Bill(
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
