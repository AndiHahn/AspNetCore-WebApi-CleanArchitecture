using System;
using System.Collections.Generic;
using CleanArchitecture.Shopping.Core.BankAccount;
using CleanArchitecture.Shopping.Core.Bill;
using CSharpFunctionalExtensions;

#nullable enable

namespace CleanArchitecture.Shopping.Core.User
{
    public class User : Entity<Guid>
    {
        private readonly List<UserBill> sharedBills = new List<UserBill>();

        private readonly List<UserBankAccount> sharedAccounts = new List<UserBankAccount>();

        private readonly List<BankAccount.BankAccount> ownedAccounts = new List<BankAccount.BankAccount>();

        private readonly List<Bill.Bill> createdBills = new List<Bill.Bill>();

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

        public virtual IReadOnlyList<BankAccount.BankAccount> OwnedAccounts => this.ownedAccounts.AsReadOnly();

        public virtual IReadOnlyList<Bill.Bill> CreatedBills => this.createdBills.AsReadOnly();

        public BankAccount.BankAccount CreateAccount(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"{nameof(name)} must not be null or empty.");
            }

            var account = new BankAccount.BankAccount(name, this);

            this.sharedAccounts.Add(new UserBankAccount(account, this));

            return account;
        }
    }
}