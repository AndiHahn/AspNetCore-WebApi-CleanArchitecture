using CleanArchitecture.Core.Helper;
using CleanArchitecture.Services.FixedCost.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Services.FixedCost.Models;

namespace CleanArchitecture.Services.FixedCost
{
    public class FixedCostService : IFixedCostService
    {
        private readonly IBudgetContext context;
        private readonly ILogger<FixedCostService> logger;

        public FixedCostService(IBudgetContext context, ILogger<FixedCostService> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<FixedCostModel>> GetByAccountIdsAsync(IEnumerable<int> accountIds)
        {
            logger.LogInformation("Get By account ids...");
            var results = new List<FixedCostModel>();
            foreach (var accountId in accountIds)
            {
                logger.LogInformation("Get by account id {id}", accountId);
                results.AddRange(await GetByAccountId(accountId));
            }
            logger.LogInformation("Successfully retrieved fixed costs...");
            return results;
        }

        public async Task<FixedCostModel> AddFixedCostAsync(FixedCostCreateModel createModel)
        {
            logger.LogInformation("Add fixed cost entity...");
            var fixedCostEntity = createModel.ToEntity();
            context.FixedCost.Add(fixedCostEntity);
            await context.SaveChangesAsync();
            logger.LogInformation("Successfully created fixed cost entity");
            return (await context.FixedCost.Include(f => f.Account)
                    .FirstOrDefaultAsync(f => f.Id == fixedCostEntity.Id)).ToModel();
        }

        public async Task DeleteFixedCostAsync(int id)
        {
            logger.LogInformation("Delete fixed cost...");
            var entity = (await context.FixedCost.FindAsync(id)).AssertEntityFound(id);
            context.FixedCost.Remove(entity);
            await context.SaveChangesAsync();
            logger.LogInformation("Successfully deleted entity");
        }

        private async Task<IEnumerable<FixedCostModel>> GetByAccountId(int accountId)
        {
            (await context.Account.FindAsync(accountId)).AssertEntityFound(accountId);
            return (await context.FixedCost.Include(f => f.Account)
                                           .Where(f => f.AccountId == accountId)
                                           .OrderByDescending(f => f.Value)
                                           .ToListAsync()).ToModels();
        }
    }
}