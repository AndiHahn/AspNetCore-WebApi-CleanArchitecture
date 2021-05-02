using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Validations;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using MediatR;

namespace CleanArchitecture.Application.UseCases.Collaboration.Commands
{
    public class ShareBillWithUserCommand : IRequest
    {
        public Guid BillId { get; }
        public Guid ShareWithUserId { get; }
        public Guid CurrentUserId { get; }

        public ShareBillWithUserCommand(Guid billId, Guid shareWithUserId, Guid currentUserId)
        {
            BillId = billId;
            ShareWithUserId = shareWithUserId;
            CurrentUserId = currentUserId;
        }
    }

    public class ShareBillWithUserCommandHandler : IRequestHandler<ShareBillWithUserCommand>
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
            var bill = await EnsureCurrentUserHasAccessToBillAsync(request.BillId, request.CurrentUserId);
            var user = (await userRepository.GetByIdAsync(request.ShareWithUserId))
                .AssertEntityFound(request.ShareWithUserId);

            if (bill.UserBills.Any(ua => ua.UserId == request.ShareWithUserId))
            {
                throw new BadRequestException($"User {request.ShareWithUserId} already has access to bill {request.BillId}");
            }

            bill.UserBills.Add(new UserBillEntity
            {
                BillId = bill.Id,
                UserId = user.Id
            });

            await billRepository.UpdateAsync(bill);

            return Unit.Value;
        }

        private async Task<BillEntity> EnsureCurrentUserHasAccessToBillAsync(
            Guid billId,
            Guid currentUserId)
        {
            var bill = (await billRepository.GetByIdWithUsersAsync(billId))
                .AssertEntityFound(billId);

            if (bill.UserBills.All(ub => ub.UserId != currentUserId))
            {
                throw new ForbiddenException($"Current user does not have access to bill {billId}");
            }

            return bill;
        }
    }
}
