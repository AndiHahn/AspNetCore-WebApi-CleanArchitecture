using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shopping.Application.Bill.Queries
{
    public class GetBillByIdQuery : IQuery<Result<BillDto>>
    {
        public GetBillByIdQuery(Guid currentUserId, Guid billId)
        {
            this.CurrentUserId = currentUserId;
            this.BillId = billId;
        }

        public Guid CurrentUserId { get; }

        public Guid BillId { get; }
    }

    internal class GetBillByIdQueryHandler : IQueryHandler<GetBillByIdQuery, Result<BillDto>>
    {
        private readonly IMapper mapper;
        private readonly IShoppingDbContext dbContext;

        public GetBillByIdQueryHandler(
            IMapper mapper,
            IShoppingDbContext dbContext)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result<BillDto>> Handle(GetBillByIdQuery request, CancellationToken cancellationToken)
        {
            var bill = await this.dbContext.Bill
                .Include(b => b.SharedWithUsers)
                .FirstOrDefaultAsync(b => b.Id == request.BillId, cancellationToken);
            if (bill is null)
            {
                return Result<BillDto>.NotFound($"Bill with id {request.BillId} not found.");
            }

            if (bill.CreatedByUserId != request.CurrentUserId &&
                bill.SharedWithUsers.All(ub => ub.UserId != request.CurrentUserId))
            {
                return Result<BillDto>.Forbidden();
            }

            return Result<BillDto>.Success(this.mapper.Map<BillDto>(bill));
        }
    }
}
