using CleanArchitecture.Core.Interfaces.Models.QueryParameter;

namespace CleanArchitecture.Core.Interfaces.Services.User.Models
{
    public class UserQueryParameter : IPagingParameter
    {
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 0;
    }
}