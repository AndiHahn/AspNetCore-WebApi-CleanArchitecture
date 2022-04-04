using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shopping.Application.Bill.Commands.DeleteBill
{
    internal class DeleteBillCommandHandler : ICommandHandler<DeleteBillCommand, Result>
    {
        private readonly IShoppingDbContext dbContext;
        private readonly IBillImageRepository billImageRepository;

        public DeleteBillCommandHandler(
            IShoppingDbContext dbContext,
            IBillImageRepository billImageRepository)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.billImageRepository = billImageRepository ?? throw new ArgumentNullException(nameof(billImageRepository));
        }

        public async Task<Result> Handle(DeleteBillCommand request, CancellationToken cancellationToken)
        {
            var bill = await this.dbContext.Bill.FindByIdAsync(request.BillId, cancellationToken);
            if (bill is null)
            {
                return Result.NotFound($"Bill with id {request.BillId} not found.");
            }

            if (bill.CreatedByUserId != request.CurrentUserId)
            {
                return Result.Forbidden($"Current user has no access to bill {request.BillId}");
            }

            if (await this.billImageRepository.ImageExistsAsync(request.BillId, cancellationToken))
            {
                await this.billImageRepository.DeleteImageAsync(request.BillId, cancellationToken);
            }

            this.dbContext.Bill.Remove(bill);

            await this.dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
