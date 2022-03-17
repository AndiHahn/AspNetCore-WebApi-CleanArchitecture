using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Shopping.Application.Bill.Queries
{
    public class GetBillTimeRangesQuery : IQuery<Result<TimeRangeDto>>
    {
    }

    internal class GetBillTimeRangesQueryHandler : IQueryHandler<GetBillTimeRangesQuery, Result<TimeRangeDto>>
    {
        private readonly IShoppingDbContext dbContext;

        public GetBillTimeRangesQueryHandler(
            IShoppingDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result<TimeRangeDto>> Handle(GetBillTimeRangesQuery request, CancellationToken cancellationToken)
        {
            var result = await this.dbContext.Bill
                .Select(b => new Tuple<DateTime, DateTime>(
                    this.dbContext.Bill.Min(b => b.Date),
                    this.dbContext.Bill.Max(b => b.Date)
                ))
                .FirstOrDefaultAsync(cancellationToken);
            if (result is null)
            {
                return new TimeRangeDto();
            }

            return new TimeRangeDto(result.Item1, result.Item2);
        }
    }
}
