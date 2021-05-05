using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Repositories.Sql
{
    public class BankAccountRepository : EfRepository<BankAccountEntity>, IBankAccountRepository
    {
        private readonly IBudgetContext context;

        public BankAccountRepository(IBudgetContext context)
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
    }
}
