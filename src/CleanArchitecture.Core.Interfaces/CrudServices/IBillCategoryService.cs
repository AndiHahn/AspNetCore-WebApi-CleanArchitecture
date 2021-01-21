using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Models.Domain.BillCategory;

namespace CleanArchitecture.Core.Interfaces.CrudServices
{
    public interface IBillCategoryService
    {
        Task<IEnumerable<BillCategoryModel>> GetAllAsync();
        Task<BillCategoryModel> GetByIdAsync(Guid id);
        Task<BillCategoryModel> CreateAsync(BillCategoryCreateModel createModel);
        Task UpdateAsync(Guid id, BillCategoryUpdateModel updateModel);
        Task DeleteAsync(Guid id);
    }
}
