using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Models.Result;
using CleanArchitecture.Shopping.Core.Bill;
using CleanArchitecture.Shopping.Core.Interfaces;

#nullable enable

namespace CleanArchitecture.Shopping.Application.Bill.Commands
{
    public class UpdateBillCommand : ICommand<Result<BillDto>>
    {
        public UpdateBillCommand(
            Guid currentUserId,
            Guid billId,
            string? shopName,
            double? price,
            DateTime? date,
            string? notes,
            Category? category,
            byte[] version)
        {
            this.CurrentUserId = currentUserId;
            this.BillId = billId;
            this.ShopName = shopName;
            this.Price = price;
            this.Category = category;
            this.Date = date;
            this.Notes = notes;
            this.Version = version;
        }

        public Guid CurrentUserId { get; }

        public Guid BillId { get; }

        public string? ShopName { get;}

        public double? Price { get; }

        public DateTime? Date { get; }

        public string? Notes { get; }

        public Category? Category { get; }

        public byte[] Version { get; }
    }

    internal class UpdateBillCommandHandler : ICommandHandler<UpdateBillCommand, Result<BillDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UpdateBillCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<BillDto>> Handle(UpdateBillCommand request, CancellationToken cancellationToken)
        {
            var bill = await this.unitOfWork.BillRepository.GetByIdAsync(request.BillId, cancellationToken);
            if (bill == null)
            {
                return Result<BillDto>.NotFound($"Bill with id {request.BillId} not found.");
            }

            if (bill.CreatedByUserId != request.CurrentUserId)
            {
                return Result<BillDto>.Forbidden($"Current user has no access to bill {request.BillId}");
            }

            bill.Update(request.Date, request.Category, request.Price, request.ShopName, request.Notes, request.Version);

            this.unitOfWork.BillRepository.Update(bill);

            await this.unitOfWork.CommitAsync(cancellationToken);

            return this.mapper.Map<BillDto>(bill);
        }
    }
}
