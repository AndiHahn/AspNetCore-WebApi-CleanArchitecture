using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shopping.Application.Bill.Commands.ShareBillWithUser
{
    internal class ShareBillWithUserCommandHandler : ICommandHandler<ShareBillWithUserCommand, Result>
    {
        private readonly IShoppingDbContext dbContext;

        public ShareBillWithUserCommandHandler(IShoppingDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result> Handle(ShareBillWithUserCommand request, CancellationToken cancellationToken)
        {
            var bill = await this.dbContext.Bill
                .Include(b => b.SharedWithUsers)
                .FirstOrDefaultAsync(b => b.Id == request.BillId, cancellationToken);
            if (bill is null)
            {
                return Result.NotFound($"Bill with id {request.BillId} not found.");
            }

            if (!bill.HasCreated(request.CurrentUserId))
            {
                return Result.Forbidden($"Current user does not have access to bill {request.BillId}");
            }

            var user = await this.dbContext.User.FindByIdAsync(request.ShareWithUserId, cancellationToken);
            if (user is null)
            {
                return Result.NotFound($"User with id {request.ShareWithUserId} not found.");
            }

            bill.ShareWithUser(user);

            this.dbContext.Bill.Update(bill);

            await this.dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
