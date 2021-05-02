using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Database.Budget;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories.Sql
{
    public class BankAccountRepository : EfRepository<BankAccountEntity>, IBankAccountRepository
    {
        private readonly BudgetContext context;

        public BankAccountRepository(BudgetContext context)
        : base(context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<BankAccountEntity>> ListByUserAsync(Guid userId)
        {
            return await context.BankAccount
                    .Where(ba => ba.OwnerId == userId)
                    .ToListAsync();
        }

        public async Task<BankAccountEntity> GetByIdWithUsersAsync(Guid id)
        {
            return await context.BankAccount
                    .Include(a => a.UserBankAccounts)
                    .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<BankAccountEntity>> GetSharedAccountsAsync(Guid userId)
        {
            return await context.UserBankAccount
                .Include(uba => uba.BankAccount)
                .Where(uba => uba.UserId == userId && uba.BankAccount.OwnerId != userId)
                .Select(uba => uba.BankAccount)
                .ToListAsync();
        }
    }
}
