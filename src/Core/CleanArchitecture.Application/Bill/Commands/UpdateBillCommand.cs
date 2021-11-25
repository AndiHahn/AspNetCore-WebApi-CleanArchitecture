using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Core;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces;
using MediatR;

#nullable enable

namespace CleanArchitecture.Application.Bill
{
    public class UpdateBillCommand : IRequest<BillDto>
    {
        public UpdateBillCommand(
            Guid currentUserId,
            Guid billId,
            string? shopName,
            double? price,
            DateTime? date,
            string? notes,
            Category? category)
        {
            this.CurrentUserId = currentUserId;
            this.BillId = billId;
            this.ShopName = shopName;
            this.Price = price;
            this.Category = category;
            this.Date = date;
            this.Notes = notes;
        }

        public Guid CurrentUserId { get; }

        public Guid BillId { get; }

        public string? ShopName { get;}

        public double? Price { get; }

        public DateTime? Date { get; }

        public string? Notes { get; }

        public Category? Category { get; }
    }

    internal class UpdateBillCommandHandler : IRequestHandler<UpdateBillCommand, BillDto>
    {
        private readonly IBillRepository billRepository;
        private readonly IMapper mapper;

        public UpdateBillCommandHandler(
            IBillRepository billRepository,
            IMapper mapper)
        {
            this.billRepository = billRepository ?? throw new ArgumentNullException(nameof(billRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<BillDto> Handle(UpdateBillCommand request, CancellationToken cancellationToken)
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

            bill.Update(request.Date, request.Category, request.Price, request.ShopName, request.Notes);

            await this.billRepository.UpdateAsync(bill);

            return this.mapper.Map<BillDto>(bill);
        }
    }
}
