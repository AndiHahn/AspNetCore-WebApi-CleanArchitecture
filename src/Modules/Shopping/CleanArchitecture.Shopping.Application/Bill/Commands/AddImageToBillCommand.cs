using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Core.Models;
using CleanArchitecture.Shared.Core.Models.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Shopping.Application.Bill.Commands
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
        private readonly IUnitOfWork unitOfWork;
        private readonly IBillImageRepository billImageRepository;

        public AddImageToBillCommandHandler(
            IUnitOfWork unitOfWork,
            IBillImageRepository billImageRepository)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.billImageRepository = billImageRepository ?? throw new ArgumentNullException(nameof(billImageRepository));
        }

        public async Task<Result> Handle(AddImageToBillCommand request, CancellationToken cancellationToken)
        {
            var bill = await this.unitOfWork.BillRepository.GetByIdAsync(request.BillId, cancellationToken);
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
