using System;
using CleanArchitecture.Shopping.Core.BankAccount;
using CleanArchitecture.Shopping.Core.User;

namespace CleanArchitecture.Shopping.UnitTests.Builder
{
    public class AccountBuilder
    {
        private string name = "Name";
        private Guid ownerId;
        private User owner;

        public AccountBuilder Name(string name)
        {
            this.name = name;
            return this;
        }

        public AccountBuilder Owner(Guid userId)
        {
            this.ownerId = userId;
            return this;
        }

        public AccountBuilder Owner(User user)
        {
            this.owner = user;
            return this;
        }

        public BankAccount Build()
        {
            if (owner is not null)
            {
                return new BankAccount(this.name, this.owner);
            }
            else if (ownerId != Guid.Empty)
            {
                return new BankAccount(this.name, this.ownerId);
            }

            throw new InvalidOperationException($"Cannot create {nameof(BankAccount)}. Owner must be set.");
        }
    }
}
