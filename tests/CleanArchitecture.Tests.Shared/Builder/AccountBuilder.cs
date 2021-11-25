using System;

namespace CleanArchitecture.Tests.Shared.Builder
{
    public class AccountBuilder
    {
        private string name = "Name";
        private Guid ownerId;
        private Core.User owner;

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

        public AccountBuilder Owner(Core.User user)
        {
            this.owner = user;
            return this;
        }

        public Core.BankAccount Build()
        {
            if (owner is not null)
            {
                return new Core.BankAccount(this.name, this.owner);
            }
            else if (ownerId != Guid.Empty)
            {
                return new Core.BankAccount(this.name, this.ownerId);
            }

            throw new InvalidOperationException($"Cannot create {nameof(Core.BankAccount)}. Owner must be set.");
        }
    }
}
