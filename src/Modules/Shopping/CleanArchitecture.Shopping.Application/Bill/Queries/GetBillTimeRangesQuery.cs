using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Models.Result;
using CleanArchitecture.Shopping.Core.Interfaces;

namespace CleanArchitecture.Shopping.Application.Bill.Queries
{
    public class GetBillTimeRangesQuery : IQuery<Result<TimeRangeDto>>
    {
    }

    internal class GetBillTimeRangesQueryHandler : IQueryHandler<GetBillTimeRangesQuery, Result<TimeRangeDto>>
    {
        private readonly IBillRepository billRepository;

        public GetBillTimeRangesQueryHandler(
            IBillRepository billRepository)
        {
            this.billRepository = billRepository ?? throw new ArgumentNullException(nameof(billRepository));
        }

        public async Task<Result<TimeRangeDto>> Handle(GetBillTimeRangesQuery request, CancellationToken cancellationToken)
        {
            var result = await this.billRepository.GetMinAndMaxBillDateAsync(cancellationToken);

            if (result.MinDate == default || result.MaxDate == default)
            {
                return new TimeRangeDto();
            }

            return new TimeRangeDto(result.MinDate, result.MaxDate);
        }
    }
}
