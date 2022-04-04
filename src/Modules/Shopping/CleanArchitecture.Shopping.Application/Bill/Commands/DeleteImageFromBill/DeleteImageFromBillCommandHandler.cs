using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shopping.Application.Bill.Commands.DeleteImageFromBill
{
    internal class DeleteImageFromBillCommandHandler : ICommandHandler<DeleteImageFromBillCommand, Result>
    {
        private readonly IShoppingDbContext dbContext;
        private readonly IBillImageRepository billImageRepository;

        public DeleteImageFromBillCommandHandler(
            IShoppingDbContext dbContext,
            IBillImageRepository billImageRepository)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.billImageRepository = billImageRepository ?? throw new ArgumentNullException(nameof(billImageRepository));
        }

        public async Task<Result> Handle(DeleteImageFromBillCommand request, CancellationToken cancellationToken)
        {
            var bill = await this.dbContext.Bill.FindByIdAsync(request.BillId, cancellationToken);
            if (bill is null)
            {
                return Result.NotFound($"Bill with id {request.BillId} not found.");
            }

            if (!bill.HasCreated(request.CurrentUserId))
            {
                return Result.Forbidden($"Current user has no access to bill {request.BillId}");
            }

            await this.billImageRepository.DeleteImageAsync(request.BillId, cancellationToken);

            return Result.Success();
        }
    }
}
