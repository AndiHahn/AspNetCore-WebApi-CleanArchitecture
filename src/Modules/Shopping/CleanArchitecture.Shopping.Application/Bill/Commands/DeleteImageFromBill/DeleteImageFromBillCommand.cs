using System;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.Shopping.Application.Bill.Commands.DeleteImageFromBill
{
    public class DeleteImageFromBillCommand : ICommand<Result>
    {
        public DeleteImageFromBillCommand(Guid currentUserId, Guid billId)
        {
            this.CurrentUserId = currentUserId;
            this.BillId = billId;
        }

        public Guid CurrentUserId { get; }

        public Guid BillId { get; }
    }
}
