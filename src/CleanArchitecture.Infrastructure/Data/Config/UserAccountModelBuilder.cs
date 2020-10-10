using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data.Config
{
    public static class UserAccountModelBuilder
    {
        public static void ApplyModelBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccountEntity>()
                .HasKey(ua => new { ua.AccountId, ua.UserId });

            modelBuilder.Entity<UserAccountEntity>()
                .HasOne(ua => ua.Account)
                .WithMany(a => a.UserAccounts)
                .HasForeignKey(ua => ua.AccountId);

            modelBuilder.Entity<UserAccountEntity>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserAccounts)
                .HasForeignKey(ua => ua.UserId);
        }
    }
}