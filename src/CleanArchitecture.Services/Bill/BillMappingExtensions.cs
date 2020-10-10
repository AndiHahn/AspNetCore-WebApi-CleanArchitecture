using System;
using System.Collections.Generic;
using System.Linq;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Services.Bill.Models;

namespace CleanArchitecture.Services.Bill.Extensions
{
    public static class BillMappingExtensions
    {
        public static IEnumerable<BillModel> ToModels(this IEnumerable<BillEntity> entities)
        {
            return entities.Select(e => e.ToModel());
        }

        public static BillModel ToModel(this BillEntity entity)
        {
            return new BillModel()
            {
                Id = entity.Id,
                Account = entity.Account.Name,
                User = $"{entity.User.FirstName} {entity.User.LastName}",
                Category = entity.BillCategory.Name,
                ShopName = entity.ShopName,
                Price = entity.Price,
                Date = entity.Date,
                Notes = entity.Notes,
                Version = entity.Version
            };
        }

        public static BillEntity ToEntity(this BillCreateModel createModel)
        {
            return new BillEntity()
            {
                AccountId = createModel.AccountId,
                UserId = createModel.UserId,
                BillCategoryId = createModel.CategoryId,
                ShopName = createModel.ShopName,
                Price = createModel.Price,
                Date = createModel.Date ?? DateTime.UtcNow,
                Notes = createModel.Notes ?? string.Empty
            };
        }

        public static void MergeIntoEntity(this BillUpdateModel updateModel, BillEntity entity)
        {
            entity.AccountId = updateModel.AccountId ?? entity.AccountId;
            entity.UserId = updateModel.UserId ?? entity.AccountId;
            entity.BillCategoryId = updateModel.CategoryId ?? entity.BillCategoryId;
            entity.ShopName = updateModel.ShopName ?? entity.ShopName;
            entity.Price = updateModel.Price ?? entity.Price;
            entity.Date = updateModel.Date ?? entity.Date;
            entity.Notes = updateModel.Notes ?? entity.Notes;
        }
    }
}