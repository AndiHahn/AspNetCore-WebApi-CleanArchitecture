using System;
using System.Collections.Generic;
using CleanArchitecture.Core.Models.QueryParameter;

namespace CleanArchitecture.Core.Models.Domain.Bill
{
    public class BillSearchParameter : IPagingParameter
    {
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 0;
        public string Search { get; set; }
        public IEnumerable<Guid> AccountIds { get; set; }
    }
}