using CleanArchitecture.Shopping.Core.BankAccount;
using CleanArchitecture.Shopping.Core.Bill;
using CleanArchitecture.Shopping.Core.User;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shopping.Infrastructure.Database.Budget
{
    internal static class ModelBuilderExtensions
    {
        public static void ConfigureEntities(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>()
                .HasKey(b => b.Id);
            modelBuilder.Entity<BankAccount>()
                .HasOne(b => b.Owner)
                .WithMany(u => u.OwnedAccounts)
                .HasForeignKey(b => b.OwnerId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserBankAccount>()
                .HasKey(ua => new { ua.BankAccountId, ua.UserId });
            modelBuilder.Entity<UserBankAccount>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.SharedAccounts)
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserBankAccount>()
                .HasOne(ua => ua.BankAccount)
                .WithMany(b => b.SharedWithUsers)
                .HasForeignKey(ua => ua.BankAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Bill>()
                .HasKey(b => b.Id);
            modelBuilder.Entity<Bill>()
                .HasOne(b => b.BankAccount)
                .WithMany(ba => ba.Bills)
                .HasForeignKey(b => b.BankAccountId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Bill>()
                .HasOne(b => b.CreatedByUser)
                .WithMany(u => u.CreatedBills)
                .HasForeignKey(b => b.CreatedByUserId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Bill>()
                .Property(b => b.Category)
                .HasConversion(
                    c => c.Value,
                    c => Category.FromValue(c));

            modelBuilder.Entity<UserBill>()
                .HasKey(ub => new { ub.BillId, ub.UserId });
            modelBuilder.Entity<UserBill>()
                .HasOne(ub => ub.User)
                .WithMany(u => u.SharedBills)
                .HasForeignKey(ub => ub.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserBill>()
                .HasOne(ub => ub.Bill)
                .WithMany(b => b.SharedWithUsers)
                .HasForeignKey(ub => ub.BillId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id );
        }
    }
}
