using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

#nullable enable

namespace CleanArchitecture.Core
{
    public class User : Entity<Guid>
    {
        private readonly List<UserBill> sharedBills = new List<UserBill>();

        private readonly List<UserBankAccount> sharedAccounts = new List<UserBankAccount>();

        private readonly List<BankAccount> ownedAccounts = new List<BankAccount>();

        private readonly List<Bill> createdBills = new List<Bill>();

        private User()
        {
        }

        public User(Guid id, string userName)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("UserId must not be empty.", nameof(id));
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("UserName must not be null or empty.", nameof(userName));
            }

            this.Id = id;
            this.UserName = userName;
        }

        public string UserName { get; private set; }

        public virtual IReadOnlyList<UserBill> SharedBills => this.sharedBills.AsReadOnly();

        public virtual IReadOnlyList<UserBankAccount> SharedAccounts => this.sharedAccounts.AsReadOnly();

        public virtual IReadOnlyList<BankAccount> OwnedAccounts => this.ownedAccounts.AsReadOnly();

        public virtual IReadOnlyList<Bill> CreatedBills => this.createdBills.AsReadOnly();

        public BankAccount CreateAccount(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"{nameof(name)} must not be null or empty.");
            }

            var account = new BankAccount(name, this);

            this.sharedAccounts.Add(new UserBankAccount(account, this));

            return account;
        }
    }
}