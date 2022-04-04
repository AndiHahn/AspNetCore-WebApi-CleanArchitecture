using System;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.Shopping.Application.Bill.Commands.ShareBillWithUser
{
    public class ShareBillWithUserCommand : ICommand<Result>
    {
        public ShareBillWithUserCommand(Guid billId, Guid shareWithUserId, Guid currentUserId)
        {
            BillId = billId;
            ShareWithUserId = shareWithUserId;
            CurrentUserId = currentUserId;
        }

        public Guid CurrentUserId { get; }

        public Guid BillId { get; }

        public Guid ShareWithUserId { get; }
    }
}
