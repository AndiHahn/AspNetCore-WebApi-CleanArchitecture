using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IBankAccountRepository : IRepository<BankAccount>
    {
        Task<IReadOnlyList<BankAccount>> ListOwnAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<BankAccount>> ListSharedAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<BankAccount> GetByIdWithUsersAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
