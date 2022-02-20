using System;
using System.Collections.Generic;
using System.Linq;
using CleanArchitecture.Shared.Core.Interfaces.Entities;
using CSharpFunctionalExtensions;

#nullable enable

namespace CleanArchitecture.Shopping.Core.Bill
{
    public class Bill : Entity<Guid>, IVersionableEntity
    {
        private readonly List<UserBill> sharedWithUsers = new List<UserBill>();

        private Bill()
        {
        }

        public Bill(
            User.User user,
            BankAccount.BankAccount account,
            string shopName,
            double price,
            DateTime? date,
            string? notes,
            Category category)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            this.CreatedByUserId = user.Id;
            this.BankAccountId = account.Id;
            this.ShopName = shopName;
            this.Price = price;
            this.Date = date ?? DateTime.UtcNow;
            this.Notes = notes ?? string.Empty;
            this.Category = category;
            this.CreatedByUser = user;
            this.BankAccount = account;
        }

        public Guid BankAccountId { get; private set; }

        public Guid CreatedByUserId { get; private set; }

        public string ShopName { get; private set; }

        public double Price { get; private set; }

        public DateTime Date { get; private set; }

        public string Notes { get; private set; }

        public Category Category { get; private set; }

        public byte[] Version { get; set; }

        public virtual User.User CreatedByUser { get; private set; }

        public virtual BankAccount.BankAccount BankAccount {get; private set; }

        public virtual IReadOnlyCollection<UserBill> SharedWithUsers => this.sharedWithUsers.AsReadOnly();

        public void Update(DateTime? date, Category? category, double? price, string? shopName, string? notes, byte[] version)
        {
            this.Price = price ?? this.Price;
            this.Date = date ?? this.Date;
            this.Category = category ?? this.Category;
            this.ShopName = shopName ?? this.ShopName;
            this.Notes = notes ?? this.Notes;
            this.Version = version;
        }

        public void ShareWithUser(User.User user)
        {
            if (user == null)
            {
                throw new ArgumentException("User must not be empty!");
            }

            if (this.SharedWithUsers.All(ua => ua.User != user))
            {
                this.sharedWithUsers.Add(new UserBill(this, user));
            }
        }

        public bool HasCreated(Guid userId) => this.CreatedByUserId == userId;
    }
}
