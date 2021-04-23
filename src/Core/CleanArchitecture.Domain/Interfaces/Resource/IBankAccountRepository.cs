using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Domain.Interfaces
{
    public interface IBankAccountRepository : IRepository<BankAccountEntity>
    {
        Task<IEnumerable<BankAccountEntity>> ListByUserAsync(Guid userId);
        Task<BankAccountEntity> GetByIdWithUsersAsync(Guid id);
        Task<IEnumerable<BankAccountEntity>> GetSharedAccountsAsync(Guid userId);
    }
}
