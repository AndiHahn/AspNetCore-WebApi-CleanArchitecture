namespace CleanArchitecture.Web.Api.Dtos
{
    public class SearchBillsParameters
    {
        public int PageSize { get; set; } = 100;

        public int PageIndex { get; set; } = 0;

        public bool IncludeShared { get; set; } = false;
        
        public string Search { get; set; }

        public override string ToString()
            => $"{nameof(PageSize)} = {PageSize}, {nameof(PageIndex)} = {PageIndex}, {nameof(IncludeShared)} = {IncludeShared}, {nameof(Search)} = {Search}";
    }
}
