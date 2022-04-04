using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.Shopping.Application.User.Queries.AuthenticateUser
{
    public class AuthenticateUserQuery : IQuery<Result<AuthenticationResponseDto>>
    {
        public AuthenticateUserQuery(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public string Username { get; }

        public string Password { get; }
    }
}
