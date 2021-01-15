using System;
using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Core.Interfaces.Services.Bill.Models
{
    public class BillUpdateModel
    {
#nullable enable
        public int? AccountId { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }
        public string? ShopName { get; set; }
        public double? Price { get; set; }
        public DateTime? Date { get; set; }
        public string? Notes { get; set; }
#nullable disable
        public byte[] Version { get; set; }

        public void MergeIntoEntity(BillEntity entity)
        {
            entity.AccountId = AccountId ?? entity.AccountId;
            entity.UserId = UserId ?? entity.AccountId;
            entity.BillCategoryId = CategoryId ?? entity.BillCategoryId;
            entity.ShopName = ShopName ?? entity.ShopName;
            entity.Price = Price ?? entity.Price;
            entity.Date = Date ?? entity.Date;
            entity.Notes = Notes ?? entity.Notes;
            entity.Version = Version;
        }
    }
}