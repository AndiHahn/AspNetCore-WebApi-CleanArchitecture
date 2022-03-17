using CleanArchitecture.Shared.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shopping.Core.Interfaces
{
    public interface IShoppingDbContext : IDbContext
    {
        public DbSet<Bill> Bill { get; }

        public DbSet<User> User { get; }

        public DbSet<BankAccount> BankAccount { get; }

        public DbSet<UserBankAccount> UserBankAccount { get; }

        public DbSet<UserBill> UserBill { get; }
    }
}
