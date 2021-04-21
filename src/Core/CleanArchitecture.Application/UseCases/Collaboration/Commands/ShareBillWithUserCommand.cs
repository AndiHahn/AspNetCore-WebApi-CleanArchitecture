using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Validations;
using CleanArchitecture.Core.Interfaces.Data;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        private readonly IBudgetContext context;

        public ShareBillWithUserCommandHandler(
            IBudgetContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Unit> Handle(ShareBillWithUserCommand request, CancellationToken cancellationToken)
        {
            var bill = await EnsureCurrentUserHasAccessToBillAsync(request.BillId, request.CurrentUserId);
            var user = (await context.User.FindAsync(request.ShareWithUserId))
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

            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task<BillEntity> EnsureCurrentUserHasAccessToBillAsync(
            Guid billId,
            Guid currentUserId)
        {
            var bill = (await context.Bill
                    .Include(b => b.UserBills)
                    .FirstOrDefaultAsync(b => b.Id == billId))
                .AssertEntityFound(billId);

            if (bill.UserBills.All(ub => ub.UserId != currentUserId))
            {
                throw new ForbiddenException($"Current user does not have access to bill {billId}");
            }

            return bill;
        }
    }
}
