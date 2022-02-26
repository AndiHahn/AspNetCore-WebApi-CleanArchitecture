namespace CleanArchitecture.Shared.Core.Models.Result
{
    public class PagedResult<T> : Result<IEnumerable<T>>, IPagedResult
        where T : class
    {
        public PagedResult(IEnumerable<T> value, int totalCount) : base(value)
        {
            this.TotalCount = totalCount;
        }

        public int TotalCount { get; }
    }
}
