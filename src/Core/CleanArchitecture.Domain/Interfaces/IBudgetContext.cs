using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanArchitecture.Domain.Interfaces
{
    public interface IBudgetContext
    {
        DbSet<BillEntity> Bill { get; }
        DbSet<UserEntity> User { get; }
        DbSet<BankAccountEntity> BankAccount { get; }
        DbSet<UserBankAccountEntity> UserBankAccount { get; }
        DbSet<UserBillEntity> UserBill { get; }

        Task MigrateAsync();
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<T> Set<T>() where T : class;
        EntityEntry Entry(object entity);
    }
}
