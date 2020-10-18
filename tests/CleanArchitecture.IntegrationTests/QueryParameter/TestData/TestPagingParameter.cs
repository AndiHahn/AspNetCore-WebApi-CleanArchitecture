using CleanArchitecture.Core.Interfaces.Models.QueryParameter;

namespace CleanArchitecture.IntegrationTests.QueryParameter.TestData
{
    public class TestPagingParameter : IPagingParameter
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public TestPagingParameter(int pageSize, int pageIndex)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
        }
    }
}
