using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Shopping.Core.Interfaces
{
    public interface IBankAccountRepository : IRepository<BankAccount.BankAccount>
    {
        Task<IReadOnlyList<BankAccount.BankAccount>> ListOwnAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<BankAccount.BankAccount>> ListSharedAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<BankAccount.BankAccount> GetByIdWithUsersAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
