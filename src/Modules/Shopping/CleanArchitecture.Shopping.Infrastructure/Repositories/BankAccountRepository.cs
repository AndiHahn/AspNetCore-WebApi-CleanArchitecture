﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shopping.Core.BankAccount;
using CleanArchitecture.Shopping.Core.Interfaces;
using CleanArchitecture.Shopping.Infrastructure.Database.Budget;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shopping.Infrastructure.Repositories
{
    internal class BankAccountRepository : EfRepository<BankAccount>, IBankAccountRepository
    {
        private readonly ShoppingDbContext context;

        public BankAccountRepository(ShoppingDbContext context)
        : base(context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IReadOnlyList<BankAccount>> ListOwnAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await context.BankAccount
                     .Where(ba => ba.OwnerId == userId)
                     .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<BankAccount>> ListSharedAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await context.BankAccount
                    .Where(ba => ba.SharedWithUsers.Any(ub => ub.UserId == userId))
                    .ToListAsync(cancellationToken);
        }

        public Task<BankAccount> GetByIdWithUsersAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return context.BankAccount
                    .Include(a => a.SharedWithUsers)
                    .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }
    }
}