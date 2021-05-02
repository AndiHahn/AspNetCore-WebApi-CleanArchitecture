using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Application.CrudServices.Models.BankAccount;

namespace CleanArchitecture.Application.CrudServices.Interfaces
{
    public interface IBankAccountService
    {
        Task<IEnumerable<BankAccountModel>> GetAllAsync(Guid currentUserId);
        Task<BankAccountModel> GetByIdAsync(Guid id, Guid currentUserId);
        Task<BankAccountModel> CreateAccountAsync(BankAccountCreateModel createModel, Guid currentUserId);
    }
}
