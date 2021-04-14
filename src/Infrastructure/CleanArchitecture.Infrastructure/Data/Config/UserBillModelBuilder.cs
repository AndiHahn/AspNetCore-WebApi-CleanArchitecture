using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Data.Config
{
    public static class UserBillModelBuilder
    {
        public static void ApplyModelBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserBillEntity>()
                .HasKey(ub => new { ub.UserId, ub.BillId });

            modelBuilder.Entity<UserBillEntity>()
                .HasOne(ub => ub.User)
                .WithMany(u => u.UserBills)
                .HasForeignKey(ub => ub.UserId);

            modelBuilder.Entity<UserBillEntity>()
                .HasOne(ub => ub.Bill)
                .WithMany(b => b.UserBills)
                .HasForeignKey(ub => ub.BillId);
        }
    }
}
