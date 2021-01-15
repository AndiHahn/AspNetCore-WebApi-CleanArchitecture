using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Core.Interfaces.Infrastructure;
using CleanArchitecture.Core.Interfaces.Services.Income;
using CleanArchitecture.Core.Interfaces.Services.Income.Models;
using CleanArchitecture.Core.Validations;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Core.CrudServices
{
    public class IncomeService : IIncomeService
    {
        private readonly IBudgetContext context;
        private readonly IMapper mapper;
        private readonly ILogger<IncomeService> logger;

        public IncomeService(
                IBudgetContext context,
                IMapper mapper,
                ILogger<IncomeService> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<IncomeModel>> GetByAccountIdsAsync(IEnumerable<int> accountIds)
        {
            logger.LogInformation("Get by account ids...");
            var results = new List<IncomeModel>();
            foreach (var accountId in accountIds)
            {
                logger.LogInformation("Get by account id {id}", accountId);
                results.AddRange(await GetByAccountId(accountId));
            }
            logger.LogInformation("Successfully retrieved incomes.");
            return results;
        }

        public async Task<IncomeModel> AddIncomeAsync(IncomeCreateModel createModel)
        {
            logger.LogInformation("Add income...");
            var incomeEntity = mapper.Map<IncomeEntity>(createModel);
            context.Income.Add(incomeEntity);
            await context.SaveChangesAsync();
            logger.LogInformation("Successfully created income.");
            var entity = await context.Income.Include(i => i.Account)
                    .Where(i => i.Id == incomeEntity.Id).FirstOrDefaultAsync();
            return mapper.Map<IncomeModel>(entity);
        }

        public async Task DeleteIncomeAsync(int id)
        {
            logger.LogInformation("Delete income...");
            var entity = (await context.Income.FindAsync(id)).AssertEntityFound(id);
            context.Income.Remove(entity);
            await context.SaveChangesAsync();
            logger.LogInformation("Successfully deleted entity");
        }

        private async Task<IEnumerable<IncomeModel>> GetByAccountId(int accountId)
        {
            (await context.Account.FindAsync(accountId)).AssertEntityFound(accountId);
            return (await context.Income.Include(i => i.Account)
                                        .Where(i => i.AccountId == accountId)
                                        .OrderByDescending(i => i.Value)
                                        .ToListAsync())
                                        .Select(i => mapper.Map<IncomeModel>(i));
        }
    }
}
