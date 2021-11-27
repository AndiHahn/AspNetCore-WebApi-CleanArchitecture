using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Bill
{
    public class DeleteBillCommand : IRequest<Result>
    {
        public DeleteBillCommand(Guid currentUserId, Guid billId)
        {
            this.CurrentUserId = currentUserId;
            this.BillId = billId;
        }

        public Guid CurrentUserId { get; }

        public Guid BillId { get; }
    }

    internal class DeleteBillCommandHandler : IRequestHandler<DeleteBillCommand, Result>
    {
        private readonly IBillRepository billRepository;
        private readonly IBillImageRepository billImageRepository;

        public DeleteBillCommandHandler(
            IBillRepository billRepository,
            IBillImageRepository billImageRepository)
        {
            this.billRepository = billRepository ?? throw new ArgumentNullException(nameof(billRepository));
            this.billImageRepository = billImageRepository ?? throw new ArgumentNullException(nameof(billImageRepository));
        }

        public async Task<Result> Handle(DeleteBillCommand request, CancellationToken cancellationToken)
        {
            var bill = await billRepository.GetByIdAsync(request.BillId);
            if (bill == null)
            {
                return Result.NotFound($"Bill with id {request.BillId} not found.");
                throw new NotFoundException($"Bill with id {request.BillId} not found.");
            }

            if (bill.CreatedByUserId != request.CurrentUserId)
            {
                throw new ForbiddenException($"Current user has no access to bill {request.BillId}");
            }

            if (await this.billImageRepository.ImageExistsAsync(request.BillId, cancellationToken))
            {
                await this.billImageRepository.DeleteImageAsync(request.BillId, cancellationToken);
            }

            await this.billRepository.DeleteAsync(bill, cancellationToken);

            return Result.Success();
        }
    }
}
