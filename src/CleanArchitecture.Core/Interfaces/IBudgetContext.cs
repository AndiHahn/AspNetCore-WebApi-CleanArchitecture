﻿using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Queries;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IBudgetContext
    {
        DbSet<BillEntity> Bill { get; set; }
        DbSet<UserEntity> User { get; set; }
        DbSet<AccountEntity> Account { get; set; }
        DbSet<UserAccountEntity> UserAccount { get; set; }
        DbSet<FixedCostEntity> FixedCost { get; set; }
        DbSet<IncomeEntity> Income { get; set; }
        DbSet<BillCategoryEntity> BillCategory { get; set; }
        IBillQueries BillQueries { get; }
        Task CreateAndMigrateAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry Entry(object entity);
    }
}