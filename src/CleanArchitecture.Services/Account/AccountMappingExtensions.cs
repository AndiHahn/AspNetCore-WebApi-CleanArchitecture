using System.Collections.Generic;
using System.Linq;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Services.Account.Models;

namespace CleanArchitecture.Services.Account.Extensions
{
    public static class AccountMappingExtensions
    {
        public static IEnumerable<AccountModel> ToModels(this IEnumerable<AccountEntity> entities)
        {
            return entities.Select(e => e.ToModel());
        }

        public static AccountModel ToModel(this AccountEntity entity)
        {
            return new AccountModel()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
