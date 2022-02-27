﻿namespace CleanArchitecture.Shared.Core.Result
{
    public class PagedResultDto
    {
        public PagedResultDto(object values, int totalCount)
        {
            Values = values;
            TotalCount = totalCount;
        }

        public object Values { get; set; }

        public int TotalCount { get; set; }
    }
}