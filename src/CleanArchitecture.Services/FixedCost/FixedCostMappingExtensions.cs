using System.Collections.Generic;
using System.Linq;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Services.FixedCost.Models;

namespace CleanArchitecture.Services.FixedCost.Extensions
{
    public static class FixedCostMappingExtensions
    {
        public static IEnumerable<FixedCostModel> ToModels(this IEnumerable<FixedCostEntity> entities)
        {
            return entities.Select(e => e.ToModel());
        }

        public static FixedCostModel ToModel(this FixedCostEntity entity)
        {
            return new FixedCostModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Value = entity.Value,
                Duration = entity.Duration,
                CostCategory = entity.CostCategory,
                AccountName = entity.Account.Name
            };
        }

        public static FixedCostEntity ToEntity(this FixedCostCreateModel createModel)
        {
            return new FixedCostEntity()
            {
                AccountId = createModel.AccountId,
                Name = createModel.Name,
                Value = createModel.Value,
                Duration = createModel.Duration,
                CostCategory = createModel.CostCategory
            };
        }
    }
}
