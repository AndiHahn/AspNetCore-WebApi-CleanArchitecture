using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Validations;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.CrudServices;
using CleanArchitecture.Core.Models.Domain.BillCategory;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.CrudServices
{
    public class BillCategoryService : IBillCategoryService
    {
        private readonly IBudgetContext context;
        private readonly IMapper mapper;
        private readonly ILogger<BillCategoryService> logger;

        public BillCategoryService(
                    IBudgetContext context,
                    IMapper mapper,
                    ILogger<BillCategoryService> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<BillCategoryModel>> GetAllAsync()
        {
            logger.LogInformation("Get all bill categories...");
            return (await context.BillCategory.ToListAsync())
                                .Select(b => mapper.Map<BillCategoryModel>(b));
        }

        public async Task<BillCategoryModel> GetByIdAsync(Guid id)
        {
            logger.LogInformation("Get bill category by id {id}", id);
            var entity = (await context.BillCategory.FindAsync(id)).AssertEntityFound(id);
            return mapper.Map<BillCategoryModel>(entity);
        }

        public async Task<BillCategoryModel> CreateAsync(BillCategoryCreateModel createModel)
        {
            logger.LogInformation("Create new bill category...");
            var entity = mapper.Map<BillCategoryEntity>(createModel);
            context.BillCategory.Add(entity);
            await context.SaveChangesAsync();
            logger.LogInformation("Successfully created bill category.");
            return await GetByIdAsync(entity.Id);
        }

        public async Task UpdateAsync(Guid id, BillCategoryUpdateModel updateModel)
        {
            logger.LogInformation("Update bill category with id {id}", id);
            var entity = (await context.BillCategory.FindAsync(id)).AssertEntityFound(id);
            updateModel.MergeIntoEntity(entity);
            await context.SaveChangesAsync();
            logger.LogInformation("Successfully updated bill category.");
        }

        public async Task DeleteAsync(Guid id)
        {
            logger.LogInformation("Delete bill category with id {id}", id);
            var entity = (await context.BillCategory.FindAsync(id)).AssertEntityFound(id);
            context.BillCategory.Remove(entity);
            await context.SaveChangesAsync();
            logger.LogInformation("Successfully deleted bill category.");
        }
    }
}
