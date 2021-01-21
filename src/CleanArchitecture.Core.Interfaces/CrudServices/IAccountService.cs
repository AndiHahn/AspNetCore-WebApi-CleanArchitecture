using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Models.Domain.Account;

namespace CleanArchitecture.Core.Interfaces.CrudServices
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountModel>> GetAllAsync();
        Task<AccountModel> GetByIdAsync(Guid id);
    }
}
