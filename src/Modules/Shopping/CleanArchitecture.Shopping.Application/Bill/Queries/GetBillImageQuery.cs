using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Models;
using CleanArchitecture.Shared.Core.Models.Result;
using CleanArchitecture.Shopping.Core.Interfaces;

namespace CleanArchitecture.Shopping.Application.Bill.Queries
{
    public class GetBillImageQuery : IQuery<Result<Blob>>
    {
        public GetBillImageQuery(Guid currentUserId, Guid billId)
        {
            this.CurrentUserId = currentUserId;
            this.BillId = billId;
        }

        public Guid CurrentUserId { get; }

        public Guid BillId { get; }
    }

    internal class GetBillImageQueryHandler : IQueryHandler<GetBillImageQuery, Result<Blob>>
    {
        private readonly IBillRepository billRepository;
        private readonly IBillImageRepository billImageRepository;

        public GetBillImageQueryHandler(
            IBillRepository billRepository,
            IBillImageRepository billImageRepository)
        {
            this.billRepository = billRepository ?? throw new ArgumentNullException(nameof(billRepository));
            this.billImageRepository = billImageRepository ?? throw new ArgumentNullException(nameof(billImageRepository));
        }

        public async Task<Result<Blob>> Handle(GetBillImageQuery request, CancellationToken cancellationToken)
        {
            var bill = await this.billRepository.GetByIdAsync(request.BillId, cancellationToken);
            if (bill is null)
            {
                return Result<Blob>.NotFound();
            }

            if (!bill.HasCreated(request.CurrentUserId))
            {
                return Result<Blob>.Forbidden($"Current user has no access to bill {request.BillId}");
            }

            var image = await this.billImageRepository.DownloadImageAsync(request.BillId, cancellationToken);
            if (image is null)
            {
                return Result<Blob>.NotFound($"Image for bill {request.BillId} not found.");
            }

            return image;
        }
    }
}
