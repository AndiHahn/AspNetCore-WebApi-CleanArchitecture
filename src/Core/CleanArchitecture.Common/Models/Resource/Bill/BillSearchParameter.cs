using CleanArchitecture.Common.Models.Query;

namespace CleanArchitecture.Common.Models.Resource.Bill
{
    public class BillSearchParameter : IPagingParameter
    {
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 0;
        public string Search { get; set; }
    }
}