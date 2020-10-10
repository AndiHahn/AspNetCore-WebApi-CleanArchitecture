using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Helper;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Services.BillCategory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Services.BillCategory
{
    public class BillCategoryService : IBillCategoryService
    {
        private readonly IBudgetContext context;
        private readonly ILogger<BillCategoryService> logger;

        public BillCategoryService(IBudgetContext context, ILogger<BillCategoryService> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<BillCategoryModel>> GetAllAsync()
        {
            logger.LogInformation("Get all bill categories...");
            return (await context.BillCategory.ToListAsync()).ToModels();
        }

        public async Task<BillCategoryModel> GetByIdAsync(int id)
        {
            logger.LogInformation("Get bill category by id {id}", id);
            return (await context.BillCategory.FindAsync(id)).AssertEntityFound(id).ToModel();
        }

        public async Task<BillCategoryModel> CreateAsync(BillCategoryCreateModel createModel)
        {
            logger.LogInformation("Create new bill category...");
            var entity = createModel.ToEntity();
            context.BillCategory.Add(entity);
            await context.SaveChangesAsync();
            logger.LogInformation("Successfully created bill category.");
            return await GetByIdAsync(entity.Id);
        }

        public async Task UpdateAsync(int id, BillCategoryUpdateModel updateModel)
        {
            logger.LogInformation("Update bill category with id {id}", id);
            var entity = (await context.BillCategory.FindAsync(id)).AssertEntityFound(id);
            updateModel.MergeIntoEntity(entity);
            await context.SaveChangesAsync();
            logger.LogInformation("Successfully updated bill category.");
        }

        public async Task DeleteAsync(int id)
        {
            logger.LogInformation("Delete bill category with id {id}", id);
            var entity = (await context.BillCategory.FindAsync(id)).AssertEntityFound(id);
            context.BillCategory.Remove(entity);
            await context.SaveChangesAsync();
            logger.LogInformation("Successfully deleted bill category.");
        }
    }
}
