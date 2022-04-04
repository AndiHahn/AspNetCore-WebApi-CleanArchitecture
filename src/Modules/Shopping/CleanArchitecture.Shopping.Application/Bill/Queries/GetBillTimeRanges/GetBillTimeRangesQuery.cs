using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.Shopping.Application.Bill.Queries.GetBillTimeRanges
{
    public class GetBillTimeRangesQuery : IQuery<Result<TimeRangeDto>>
    {
    }
}
