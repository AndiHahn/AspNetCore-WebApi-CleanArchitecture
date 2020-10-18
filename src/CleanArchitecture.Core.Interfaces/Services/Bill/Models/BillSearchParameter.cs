using System.Collections.Generic;
using CleanArchitecture.Core.Interfaces.Models.QueryParameter;

namespace CleanArchitecture.Core.Interfaces.Services.Bill.Models
{
    public class BillSearchParameter : IPagingParameter
    {
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 0;
        public string Search { get; set; }
        public IEnumerable<int> AccountIds { get; set; }
    }
}