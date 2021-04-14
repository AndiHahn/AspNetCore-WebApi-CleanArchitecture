using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.SqlQueries;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Core.Interfaces.Data
{
    public interface IBudgetContext
    {
        DbSet<BillEntity> Bill { get; }
        DbSet<UserEntity> User { get; }
        DbSet<BankAccountEntity> BankAccount { get; }
        DbSet<UserBankAccountEntity> UserBankAccount { get; }
        DbSet<UserBillEntity> UserBill { get; }
        IBillQueries BillQueries { get; }
        Task MigrateAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
