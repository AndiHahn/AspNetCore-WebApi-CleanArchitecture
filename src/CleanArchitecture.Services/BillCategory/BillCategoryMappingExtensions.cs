using System.Collections.Generic;
using System.Linq;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Services.BillCategory.Models;

namespace CleanArchitecture.Services.BillCategory
{
    public static class BillCategoryMappingExtensions
    {
        public static IEnumerable<BillCategoryModel> ToModels(this IEnumerable<BillCategoryEntity> entities)
        {
            return entities.Select(e => e.ToModel());
        }

        public static BillCategoryModel ToModel(this BillCategoryEntity entity)
        {
            return new BillCategoryModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Color = entity.Color
            };
        }

        public static BillCategoryEntity ToEntity(this BillCategoryCreateModel createModel)
        {
            return new BillCategoryEntity()
            {
                Name = createModel.Name,
                Color = createModel.Color
            };
        }

        public static void MergeIntoEntity(this BillCategoryUpdateModel updateModel, BillCategoryEntity entity)
        {
            entity.Name = updateModel.Name ?? entity.Name;
            entity.Color = updateModel.Color ?? entity.Color;
        }
    }
}
