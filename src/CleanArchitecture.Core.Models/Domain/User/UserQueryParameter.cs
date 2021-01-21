using CleanArchitecture.Core.Models.QueryParameter;

namespace CleanArchitecture.Core.Models.Domain.User
{
    public class UserQueryParameter : IPagingParameter
    {
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 0;
    }
}