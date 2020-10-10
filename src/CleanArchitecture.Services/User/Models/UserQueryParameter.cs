using CleanArchitecture.Core.QueryParameter.Models;

namespace CleanArchitecture.Services.User.Models
{
    public class UserQueryParameter : IPagingParameter
    {
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 0;
    }
}