using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Bill
{
    public class DeleteBillCommand : IRequest
    {
        public DeleteBillCommand(Guid currentUserId, Guid billId)
        {
            this.CurrentUserId = currentUserId;
            this.BillId = billId;
        }

        public Guid CurrentUserId { get; }

        public Guid BillId { get; }
    }

    internal class DeleteBillCommandHandler : IRequestHandler<DeleteBillCommand>
    {
        private readonly IBillRepository billRepository;

        public DeleteBillCommandHandler(
            IBillRepository billRepository)
        {
            this.billRepository = billRepository ?? throw new ArgumentNullException(nameof(billRepository));
        }

        public async Task<Unit> Handle(DeleteBillCommand request, CancellationToken cancellationToken)
        {
            var bill = await billRepository.GetByIdAsync(request.BillId);
            if (bill == null)
            {
                throw new NotFoundException($"Bill with id {request.BillId} not found.");
            }

            if (bill.CreatedByUserId != request.CurrentUserId)
            {
                throw new ForbiddenException($"Current user has no access to bill {request.BillId}");
            }

            await this.billRepository.DeleteAsync(bill, cancellationToken);

            return Unit.Value;
        }
    }
}
