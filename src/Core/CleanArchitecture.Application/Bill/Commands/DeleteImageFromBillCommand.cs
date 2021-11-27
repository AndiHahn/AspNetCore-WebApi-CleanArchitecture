using CleanArchitecture.Application.Models;
using CleanArchitecture.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Bill
{
    public class DeleteImageFromBillCommand : IRequest<Result>
    {
        public DeleteImageFromBillCommand(Guid currentUserId, Guid billId)
        {
            this.CurrentUserId = currentUserId;
            this.BillId = billId;
        }

        public Guid CurrentUserId { get; }

        public Guid BillId { get; }
    }

    internal class DeleteImageFromBillCommandHandler : IRequestHandler<DeleteImageFromBillCommand, Result>
    {
        private readonly IBillRepository billRepository;
        private readonly IBillImageRepository billImageRepository;

        public DeleteImageFromBillCommandHandler(
            IBillRepository billRepository,
            IBillImageRepository billImageRepository)
        {
            this.billRepository = billRepository ?? throw new ArgumentNullException(nameof(billRepository));
            this.billImageRepository = billImageRepository ?? throw new ArgumentNullException(nameof(billImageRepository));
        }

        public async Task<Result> Handle(DeleteImageFromBillCommand request, CancellationToken cancellationToken)
        {
            var bill = await this.billRepository.GetByIdAsync(request.BillId, cancellationToken);
            if (!bill.HasCreated(request.CurrentUserId))
            {
                return Result.Forbidden($"Current user has no access to bill {request.BillId}");
            }

            await this.billImageRepository.DeleteImageAsync(request.BillId);

            return Result.Success();
        }
    }
}
