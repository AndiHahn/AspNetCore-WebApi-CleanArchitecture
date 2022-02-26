
namespace CleanArchitecture.Shared.Core.Models.Result
{
    public interface IPagedResult : IResult
    {
        public int TotalCount { get; }
    }
}
