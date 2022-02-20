using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Models.Result;
using CleanArchitecture.Shopping.Core.Interfaces;

namespace CleanArchitecture.Shopping.Application.Bill.Commands
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

    internal class ShareBillWithUserCommandHandler : ICommandHandler<ShareBillWithUserCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;

        public ShareBillWithUserCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result> Handle(ShareBillWithUserCommand request, CancellationToken cancellationToken)
        {
            var bill = await this.unitOfWork.BillRepository.GetByIdWithUsersAsync(request.BillId, cancellationToken);
            if (bill is null)
            {
                return Result.NotFound($"Bill with id {request.BillId} not found.");
            }

            if (!bill.HasCreated(request.CurrentUserId))
            {
                return Result.Forbidden($"Current user does not have access to bill {request.BillId}");
            }

            var user = await this.unitOfWork.UserRepository.GetByIdAsync(request.ShareWithUserId, cancellationToken);
            if (user is null)
            {
                return Result.NotFound($"User with id {request.ShareWithUserId} not found.");
            }

            bill.ShareWithUser(user);

            this.unitOfWork.BillRepository.Update(bill);

            await this.unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}
