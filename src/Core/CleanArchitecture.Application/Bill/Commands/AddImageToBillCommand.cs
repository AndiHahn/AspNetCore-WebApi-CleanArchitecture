using CleanArchitecture.Application.Models;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Bill
{
    public class AddImageToBillCommand : IRequest<Result>
    {
        public AddImageToBillCommand(Guid currentUserId, Guid billId, IFormFile image)
        {
            this.CurrentUserId = currentUserId;
            this.BillId = billId;
            this.Image = image;
        }

        public Guid CurrentUserId { get; }

        public Guid BillId { get; }

        public IFormFile Image { get; }
    }

    internal class AddImageToBillCommandHandler : IRequestHandler<AddImageToBillCommand, Result>
    {
        private readonly IBillRepository billRepository;
        private readonly IBillImageRepository billImageRepository;

        public AddImageToBillCommandHandler(
            IBillRepository billRepository,
            IBillImageRepository billImageRepository)
        {
            this.billRepository = billRepository ?? throw new ArgumentNullException(nameof(billRepository));
            this.billImageRepository = billImageRepository ?? throw new ArgumentNullException(nameof(billImageRepository));
        }

        public async Task<Result> Handle(AddImageToBillCommand request, CancellationToken cancellationToken)
        {
            var bill = await this.billRepository.GetByIdAsync(request.BillId, cancellationToken);
            if (!bill.HasCreated(request.CurrentUserId))
            {
                return Result.Forbidden($"Current user has no access to bill {request.BillId}");
            }

            await using var blob = new Blob(request.Image.ContentType);
            await request.Image.CopyToAsync(blob.Content, cancellationToken);
            blob.Reset();
            await this.billImageRepository.UploadImageAsync(request.BillId, blob);

            return Result.Success();
        }
    }
}
