using System;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Models;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.Shopping.Application.Bill.Queries.GetBillImage
{
    public class GetBillImageQuery : IQuery<Result<Blob>>
    {
        public GetBillImageQuery(Guid currentUserId, Guid billId)
        {
            this.CurrentUserId = currentUserId;
            this.BillId = billId;
        }

        public Guid CurrentUserId { get; }

        public Guid BillId { get; }
    }
}
