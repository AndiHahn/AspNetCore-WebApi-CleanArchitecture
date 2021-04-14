using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Models.Domain.BankAccount;

namespace CleanArchitecture.Core.Interfaces.CrudServices
{
    public interface IBankAccountService
    {
        Task<IEnumerable<BankAccountModel>> GetAllAsync();
        Task<BankAccountModel> GetByIdAsync(Guid id);
        Task<BankAccountModel> CreateAccountAsync(BankAccountCreateModel createModel);
    }
}
