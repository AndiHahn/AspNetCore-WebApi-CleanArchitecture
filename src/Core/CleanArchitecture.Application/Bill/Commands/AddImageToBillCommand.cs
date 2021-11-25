using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Bill
{
    public class AddImageToBillCommand : IRequest
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

    internal class AddImageToBillCommandHandler : IRequestHandler<AddImageToBillCommand>
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

        public async Task<Unit> Handle(AddImageToBillCommand request, CancellationToken cancellationToken)
        {
            var bill = await this.billRepository.GetByIdAsync(request.BillId, cancellationToken);
            if (!bill.HasCreated(request.CurrentUserId))
            {
                throw new ForbiddenException($"Current user has no access to bill {request.BillId}");
            }

            await this.billImageRepository.UploadImageAsync(request.BillId, request.Image);

            return Unit.Value;
        }
    }
}
