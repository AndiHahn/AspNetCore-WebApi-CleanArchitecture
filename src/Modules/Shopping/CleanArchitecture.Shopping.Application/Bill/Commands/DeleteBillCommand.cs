using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Models.Result;
using CleanArchitecture.Shopping.Core.Interfaces;

namespace CleanArchitecture.Shopping.Application.Bill.Commands
{
    public class DeleteBillCommand : ICommand<Result>
    {
        public DeleteBillCommand(Guid currentUserId, Guid billId)
        {
            this.CurrentUserId = currentUserId;
            this.BillId = billId;
        }

        public Guid CurrentUserId { get; }

        public Guid BillId { get; }
    }

    internal class DeleteBillCommandHandler : ICommandHandler<DeleteBillCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBillImageRepository billImageRepository;

        public DeleteBillCommandHandler(
            IUnitOfWork unitOfWork,
            IBillImageRepository billImageRepository)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.billImageRepository = billImageRepository ?? throw new ArgumentNullException(nameof(billImageRepository));
        }

        public async Task<Result> Handle(DeleteBillCommand request, CancellationToken cancellationToken)
        {
            var bill = await this.unitOfWork.BillRepository.GetByIdAsync(request.BillId, cancellationToken);
            if (bill == null)
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

            this.unitOfWork.BillRepository.Delete(bill);

            await this.unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}
