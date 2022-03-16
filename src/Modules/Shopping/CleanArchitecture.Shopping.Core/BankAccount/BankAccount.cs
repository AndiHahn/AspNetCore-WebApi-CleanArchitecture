using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

#nullable enable

namespace CleanArchitecture.Shopping.Core
{
    public class BankAccount : Entity<Guid>
    {
        private readonly List<Core.Bill> bills = new List<Core.Bill>();
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

        public BankAccount(string name, Core.User owner)
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

        public Core.User Owner { get; private set; }

        public IReadOnlyCollection<Core.Bill> Bills => this.bills.AsReadOnly();

        public IReadOnlyCollection<UserBankAccount> SharedWithUsers => this.sharedWithUsers.AsReadOnly();

        public void ShareWithUser(Core.User user)
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

        public bool IsOwner(Core.User user) => this.Owner == user;

        public bool HasAccess(Guid userId) => IsOwner(userId) || this.sharedWithUsers.Any(u => u.UserId == userId);

        public bool HasAccess(Core.User user) => IsOwner(user) || this.sharedWithUsers.Any(u => u.User == user);
    }
}
