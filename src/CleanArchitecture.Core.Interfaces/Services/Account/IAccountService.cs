using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Services.Account.Models;

namespace CleanArchitecture.Core.Interfaces.Services.Account
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountModel>> GetAllAsync();
        Task<AccountModel> GetByIdAsync(Guid id);
    }
}
