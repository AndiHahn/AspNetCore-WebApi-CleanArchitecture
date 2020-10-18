namespace CleanArchitecture.Core.Interfaces.Models.QueryParameter
{
    public interface IPagingParameter
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}