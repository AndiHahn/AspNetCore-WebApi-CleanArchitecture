using CleanArchitecture.Core.Helper;
using CleanArchitecture.Services.Account.Extensions;
using CleanArchitecture.Services.Bill.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Services.Account.Models;

namespace CleanArchitecture.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IBudgetContext context;

        public AccountService(IBudgetContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<AccountModel>> GetAllAsync()
        {
            return (await context.Account.ToListAsync()).ToModels();
        }

        public async Task<AccountModel> GetById(int id)
        {
            return (await context.Account.FindAsync(id)).AssertEntityFound(id).ToModel();
        }
    }
}