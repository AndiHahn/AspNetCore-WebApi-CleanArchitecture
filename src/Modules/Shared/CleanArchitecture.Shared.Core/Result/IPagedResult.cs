
namespace CleanArchitecture.Shared.Core.Result
{
    public interface IPagedResult : IResult
    {
        public int TotalCount { get; }
    }
}
