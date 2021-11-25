using System.Collections.Generic;

namespace CleanArchitecture.Core.Models
{
    public class PagedResult<T>
    {
        public PagedResult()
        {
        }

        public PagedResult(IEnumerable<T> result, int totalCount)
        {
            Result = result;
            TotalCount = totalCount;
        }

        public IEnumerable<T> Result { get; set; }

        public int TotalCount { get; set; }
    }
}
