using CleanArchitecture.Core.Models;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Bill
{
    public class GetBillImageQuery : IRequest<Blob>
    {
        public GetBillImageQuery(Guid currentUserId, Guid billId)
        {
            this.CurrentUserId = currentUserId;
            this.BillId = billId;
        }

        public Guid CurrentUserId { get; }

        public Guid BillId { get; }
    }

    internal class GetBillImageQueryHandler : IRequestHandler<GetBillImageQuery, Blob>
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

        public async Task<Blob> Handle(GetBillImageQuery request, CancellationToken cancellationToken)
        {
            var bill = await this.billRepository.GetByIdAsync(request.BillId, cancellationToken);
            if (!bill.HasCreated(request.CurrentUserId))
            {
                throw new ForbiddenException($"Current user has no access to bill {request.BillId}");
            }

            return await this.billImageRepository.DownloadImageAsync(request.BillId, cancellationToken);
        }
    }
}
