using System.Collections.Generic;

namespace CleanArchitecture.Core.QueryParameter.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Result { get; set; }
        public int TotalCount { get; set; }

        public PagedResult()
        {
        }

        public PagedResult(IEnumerable<T> result, int totalCount)
        {
            Result = result;
            TotalCount = totalCount;
        }
    }
}
