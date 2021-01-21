﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Validations;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.CrudServices;
using CleanArchitecture.Core.Models.Domain.FixedCost;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.CrudServices
{
    public class FixedCostService : IFixedCostService
    {
        private readonly IBudgetContext context;
        private readonly IMapper mapper;
        private readonly ILogger<FixedCostService> logger;

        public FixedCostService(
                    IBudgetContext context,
                    IMapper mapper,
                    ILogger<FixedCostService> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<FixedCostModel>> GetByAccountIdsAsync(IEnumerable<Guid> accountIds)
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
            var fixedCostEntity = mapper.Map<FixedCostEntity>(createModel);
            context.FixedCost.Add(fixedCostEntity);
            await context.SaveChangesAsync();
            logger.LogInformation("Successfully created fixed cost entity");
            var entity = (await context.FixedCost.Include(f => f.Account)
                    .FirstOrDefaultAsync(f => f.Id == fixedCostEntity.Id));
            return mapper.Map<FixedCostModel>(entity);
        }

        public async Task DeleteFixedCostAsync(Guid id)
        {
            logger.LogInformation("Delete fixed cost...");
            var entity = (await context.FixedCost.FindAsync(id)).AssertEntityFound(id);
            context.FixedCost.Remove(entity);
            await context.SaveChangesAsync();
            logger.LogInformation("Successfully deleted entity");
        }

        private async Task<IEnumerable<FixedCostModel>> GetByAccountId(Guid accountId)
        {
            (await context.Account.FindAsync(accountId)).AssertEntityFound(accountId);
            return (await context.FixedCost.Include(f => f.Account)
                                           .Where(f => f.AccountId == accountId)
                                           .OrderByDescending(f => f.Value)
                                           .ToListAsync())
                                           .Select(f => mapper.Map<FixedCostModel>(f));
        }
    }
}