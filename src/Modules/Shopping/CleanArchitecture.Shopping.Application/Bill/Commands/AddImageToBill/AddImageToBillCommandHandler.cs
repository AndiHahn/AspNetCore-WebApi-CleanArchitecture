using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Models;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shopping.Application.Bill.Commands.AddImageToBill
{
    internal class AddImageToBillCommandHandler : ICommandHandler<AddImageToBillCommand, Result>
    {
        private readonly IShoppingDbContext dbContext;
        private readonly IBillImageRepository billImageRepository;

        public AddImageToBillCommandHandler(
            IShoppingDbContext dbContext,
            IBillImageRepository billImageRepository)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.billImageRepository = billImageRepository ?? throw new ArgumentNullException(nameof(billImageRepository));
        }

        public async Task<Result> Handle(AddImageToBillCommand request, CancellationToken cancellationToken)
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

            await using var blob = new Blob(request.Image.ContentType);
            await request.Image.CopyToAsync(blob.Content, cancellationToken);
            blob.Reset();
            await this.billImageRepository.UploadImageAsync(request.BillId, blob, cancellationToken);

            return Result.Success();
        }
    }
}
