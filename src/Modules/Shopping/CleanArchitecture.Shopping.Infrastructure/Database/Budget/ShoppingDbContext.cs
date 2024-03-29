﻿using CleanArchitecture.Shared.Infrastructure.Database;
using CleanArchitecture.Shopping.Core;
using CleanArchitecture.Shopping.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shopping.Infrastructure.Database.Budget
{
    public class ShoppingDbContext : BaseDbContext<ShoppingDbContext>, IShoppingDbContext
    {
        public DbSet<Bill> Bill { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<BankAccount> BankAccount { get; set; }

        public DbSet<UserBankAccount> UserBankAccount { get; set; }

        public DbSet<UserBill> UserBill { get; set; }

        public ShoppingDbContext(DbContextOptions<ShoppingDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureEntities();
        }
    }
}
