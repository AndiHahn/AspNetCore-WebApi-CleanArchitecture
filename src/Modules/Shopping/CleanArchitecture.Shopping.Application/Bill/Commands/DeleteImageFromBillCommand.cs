using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Core.Models.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using MediatR;

namespace CleanArchitecture.Shopping.Application.Bill.Commands
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
        private readonly IUnitOfWork unitOfWork;
        private readonly IBillImageRepository billImageRepository;

        public DeleteImageFromBillCommandHandler(
            IUnitOfWork unitOfWork,
            IBillImageRepository billImageRepository)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.billImageRepository = billImageRepository ?? throw new ArgumentNullException(nameof(billImageRepository));
        }

        public async Task<Result> Handle(DeleteImageFromBillCommand request, CancellationToken cancellationToken)
        {
            var bill = await this.unitOfWork.BillRepository.GetByIdAsync(request.BillId, cancellationToken);
            if (!bill.HasCreated(request.CurrentUserId))
            {
                return Result.Forbidden($"Current user has no access to bill {request.BillId}");
            }

            await this.billImageRepository.DeleteImageAsync(request.BillId, cancellationToken);

            return Result.Success();
        }
    }
}
