using System;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core;

#nullable enable

namespace CleanArchitecture.Shopping.Application.Bill.Commands.UpdateBill
{
    public class UpdateBillCommand : ICommand<Result<BillDto>>
    {
        public UpdateBillCommand(
            Guid currentUserId,
            Guid billId,
            string? shopName,
            double? price,
            DateTime? date,
            string? notes,
            Category? category,
            byte[] version)
        {
            this.CurrentUserId = currentUserId;
            this.BillId = billId;
            this.ShopName = shopName;
            this.Price = price;
            this.Category = category;
            this.Date = date;
            this.Notes = notes;
            this.Version = version;
        }

        public Guid CurrentUserId { get; }

        public Guid BillId { get; }

        public string? ShopName { get;}

        public double? Price { get; }

        public DateTime? Date { get; }

        public string? Notes { get; }

        public Category? Category { get; }

        public byte[] Version { get; }
    }
}
