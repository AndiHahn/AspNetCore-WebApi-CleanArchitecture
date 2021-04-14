namespace CleanArchitecture.Core.Models.QueryParameter
{
    public interface IPagingParameter
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}