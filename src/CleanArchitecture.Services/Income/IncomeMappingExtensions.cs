using System.Collections.Generic;
using System.Linq;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Services.Income.Models;

namespace CleanArchitecture.Services.Income.Extensions
{
    public static class IncomeMappingExtensions
    {
        public static IEnumerable<IncomeModel> ToModels(this IEnumerable<IncomeEntity> entities)
        {
            return entities.Select(e => e.ToModel());
        }

        public static IncomeModel ToModel(this IncomeEntity entity)
        {
            return new IncomeModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Value = entity.Value,
                Duration = entity.Duration,
                AccountName = entity.Account.Name
            };
        }

        public static IncomeEntity ToEntity(this IncomeCreateModel createModel)
        {
            return new IncomeEntity()
            {
                AccountId = createModel.AccountId,
                Name = createModel.Name,
                Value = createModel.Value,
                Duration = createModel.Duration
            };
        }
    }
}
