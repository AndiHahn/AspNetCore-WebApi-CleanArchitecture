using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Core.Models.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using MediatR;

namespace CleanArchitecture.Shopping.Application.Bill.Commands
{
    public class ShareBillWithUserCommand : IRequest<Result>
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

    internal class ShareBillWithUserCommandHandler : IRequestHandler<ShareBillWithUserCommand, Result>
    {
        private readonly IUserRepository userRepository;
        private readonly IBillRepository billRepository;

        public ShareBillWithUserCommandHandler(
            IUserRepository userRepository,
            IBillRepository billRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.billRepository = billRepository ?? throw new ArgumentNullException(nameof(billRepository));
        }

        public async Task<Result> Handle(ShareBillWithUserCommand request, CancellationToken cancellationToken)
        {
            var bill = await billRepository.GetByIdWithUsersAsync(request.BillId, cancellationToken);
            if (bill == null)
            {
                return Result.NotFound($"Bill with id {request.BillId} not found.");
            }

            if (!bill.HasCreated(request.CurrentUserId))
            {
                return Result.Forbidden($"Current user does not have access to bill {request.BillId}");
            }

            var user = await this.userRepository.GetByIdAsync(request.ShareWithUserId);
            if (user == null)
            {
                return Result.NotFound($"User with id {request.ShareWithUserId} not found.");
            }

            bill.ShareWithUser(user);

            await this.billRepository.UpdateAsync(bill, cancellationToken);

            return Result.Success();
        }
    }
}
