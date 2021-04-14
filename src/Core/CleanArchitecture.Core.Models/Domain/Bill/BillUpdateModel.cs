using System;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Core.Models.Domain.Bill
{
    public class BillUpdateModel
    {
#nullable enable
        public Guid? BankAccountId { get; set; }
        public string? ShopName { get; set; }
        public double? Price { get; set; }
        public DateTime? Date { get; set; }
        public string? Notes { get; set; }
        public Category? Category { get; set; }
#nullable disable
        public byte[] Version { get; set; }

        public void MergeIntoEntity(BillEntity entity)
        {
            entity.BankAccountId = BankAccountId ?? entity.BankAccountId;
            entity.Category = Category ?? entity.Category;
            entity.ShopName = ShopName ?? entity.ShopName;
            entity.Price = Price ?? entity.Price;
            entity.Date = Date ?? entity.Date;
            entity.Notes = Notes ?? entity.Notes;
            entity.Version = Version;
        }
    }
}