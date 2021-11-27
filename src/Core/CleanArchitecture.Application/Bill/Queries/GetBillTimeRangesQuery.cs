using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Core.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Bill
{
    public class GetBillTimeRangesQuery : IRequest<Result<TimeRangeDto>>
    {
    }

    internal class GetBillTimeRangesQueryHandler : IRequestHandler<GetBillTimeRangesQuery, Result<TimeRangeDto>>
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
