namespace CleanArchitecture.Core.QueryParameter.Models
{
    public interface IPagingParameter
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}