using System;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core;

#nullable enable

namespace CleanArchitecture.Shopping.Application.Bill.Commands.CreateBill
{
    public class CreateBillCommand : ICommand<Result<BillDto>>
    {
        public CreateBillCommand(
            Guid currentUserId,
            Guid bankAccountId,
            string shopName,
            double price,
            Category category,
            DateTime? date,
            string? notes)
        {
            this.CurrentUserId = currentUserId;
            this.BankAccountId = bankAccountId;
            this.ShopName = shopName;
            this.Price = price;
            this.Category = category;
            this.Date = date;
            this.Notes = notes;
        }

        public Guid CurrentUserId { get; }

        public Guid BankAccountId { get; }

        public string ShopName { get; }

        public double Price { get; }

        public Category Category { get; }

        public DateTime? Date { get; }

        public string? Notes { get; }
    }
}
