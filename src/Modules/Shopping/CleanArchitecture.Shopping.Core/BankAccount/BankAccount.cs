using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

#nullable enable

namespace CleanArchitecture.Shopping.Core.BankAccount
{
    public class BankAccount : Entity<Guid>
    {
        private readonly List<Bill.Bill> bills = new List<Bill.Bill>();
        private readonly List<UserBankAccount> sharedWithUsers = new List<UserBankAccount>();

        private BankAccount()
        {
        }

        public BankAccount(string name, Guid userId)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Bank Account must have a name!");
            }

            if (userId == Guid.Empty)
            {
                throw new ArgumentException("Bank Account must have an valid owner!");
            }

            this.Id = Guid.NewGuid();
            this.Name = name;
            this.OwnerId = userId;
        }

        public BankAccount(string name, User.User owner)
            : this(name, owner.Id)
        {
            if (owner == null)
            {
                throw new ArgumentException("Bank Account must have an owner!");
            }

            this.Owner = owner;
        }

        public Guid OwnerId { get; private set; }

        public string Name { get; private set; }

        public User.User Owner { get; private set; }

        public IReadOnlyCollection<Bill.Bill> Bills => this.bills.AsReadOnly();

        public IReadOnlyCollection<UserBankAccount> SharedWithUsers => this.sharedWithUsers.AsReadOnly();

        public void ShareWithUser(User.User user)
        {
            if (user == null)
            {
                throw new ArgumentException("User must not be empty!");
            }

            if (this.SharedWithUsers.All(ua => ua.User != user))
            {
                this.sharedWithUsers.Add(new UserBankAccount(this, user));
            }
        }

        public bool IsOwner(Guid userId) => this.OwnerId == userId;

        public bool IsOwner(User.User user) => this.Owner == user;

        public bool HasAccess(Guid userId) => IsOwner(userId) || this.sharedWithUsers.Any(u => u.UserId == userId);

        public bool HasAccess(User.User user) => IsOwner(user) || this.sharedWithUsers.Any(u => u.User == user);
    }
}
