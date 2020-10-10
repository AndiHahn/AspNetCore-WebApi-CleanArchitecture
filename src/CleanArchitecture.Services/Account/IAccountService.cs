using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Services.Account.Models;

namespace CleanArchitecture.Services.Account
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountModel>> GetAllAsync();
        Task<AccountModel> GetById(int id);
    }
}
