using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Shopping.Core.Interfaces
{
    public interface IBankAccountRepository : IRepository<Core.BankAccount>
    {
        Task<IReadOnlyList<Core.BankAccount>> ListOwnAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Core.BankAccount>> ListSharedAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<Core.BankAccount> GetByIdWithUsersAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
