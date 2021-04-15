using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Database.Budget.Config
{
    public static class UserBankAccountModelBuilder
    {
        public static void ApplyModelBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserBankAccountEntity>()
                .HasKey(ua => new { ua.BankAccountId, ua.UserId });

            modelBuilder.Entity<UserBankAccountEntity>()
                .HasOne(ua => ua.BankAccount)
                .WithMany(a => a.UserBankAccounts)
                .HasForeignKey(ua => ua.BankAccountId);

            modelBuilder.Entity<UserBankAccountEntity>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserAccounts)
                .HasForeignKey(ua => ua.UserId);
        }
    }
}