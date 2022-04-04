using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

#nullable enable

namespace CleanArchitecture.Shopping.Application.Bill.Commands.UpdateBill
{
    internal class UpdateBillCommandHandler : ICommandHandler<UpdateBillCommand, Result<BillDto>>
    {
        private readonly IShoppingDbContext dbContext;
        private readonly IMapper mapper;

        public UpdateBillCommandHandler(
            IShoppingDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<BillDto>> Handle(UpdateBillCommand request, CancellationToken cancellationToken)
        {
            var bill = await this.dbContext.Bill.FindByIdAsync(request.BillId, cancellationToken);
            if (bill is null)
            {
                return Result<BillDto>.NotFound($"Bill with id {request.BillId} not found.");
            }

            if (bill.CreatedByUserId != request.CurrentUserId)
            {
                return Result<BillDto>.Forbidden($"Current user has no access to bill {request.BillId}");
            }

            bill.Update(request.Date, request.Category, request.Price, request.ShopName, request.Notes, request.Version);

            this.dbContext.Bill.Update(bill);

            await this.dbContext.SaveChangesAsync(cancellationToken);

            return this.mapper.Map<BillDto>(bill);
        }
    }
}
