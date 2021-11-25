using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Bill
{
    public class ShareBillWithUserCommand : IRequest
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

    internal class ShareBillWithUserCommandHandler : IRequestHandler<ShareBillWithUserCommand>
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

        public async Task<Unit> Handle(ShareBillWithUserCommand request, CancellationToken cancellationToken)
        {
            var bill = await billRepository.GetByIdWithUsersAsync(request.BillId, cancellationToken);
            if (bill == null)
            {
                throw new NotFoundException($"Bill with id {request.BillId} not found.");
            }

            if (!bill.HasCreated(request.CurrentUserId))
            {
                throw new ForbiddenException($"Current user does not have access to bill {request.BillId}");
            }

            var user = await this.userRepository.GetByIdAsync(request.ShareWithUserId);
            if (user == null)
            {
                throw new NotFoundException($"User with id {request.ShareWithUserId} not found.");
            }

            bill.ShareWithUser(user);

            await this.billRepository.UpdateAsync(bill, cancellationToken);

            return Unit.Value;
        }
    }
}
