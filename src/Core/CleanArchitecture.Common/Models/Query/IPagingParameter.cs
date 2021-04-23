namespace CleanArchitecture.Common.Models.Query
{
    public interface IPagingParameter
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}