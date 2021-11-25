using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.Bill
{
    public class GetBillTimeRangesQuery : IRequest<TimeRangeDto>
    {
    }

    internal class GetBillTimeRangesQueryHandler : IRequestHandler<GetBillTimeRangesQuery, TimeRangeDto>
    {
        private readonly IBillRepository billRepository;

        public GetBillTimeRangesQueryHandler(
            IBillRepository billRepository)
        {
            this.billRepository = billRepository ?? throw new ArgumentNullException(nameof(billRepository));
        }

        public async Task<TimeRangeDto> Handle(GetBillTimeRangesQuery request, CancellationToken cancellationToken)
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
