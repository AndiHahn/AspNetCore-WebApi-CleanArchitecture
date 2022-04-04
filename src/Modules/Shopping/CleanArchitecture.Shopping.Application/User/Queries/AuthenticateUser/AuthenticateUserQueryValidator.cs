using FluentValidation;

namespace CleanArchitecture.Shopping.Application.User.Queries.AuthenticateUser
{
    public class AuthenticateUserQueryValidator : AbstractValidator<AuthenticateUserQuery>
    {
        public AuthenticateUserQueryValidator()
        {
            RuleFor(query => query.Username).NotEmpty().NotNull();
            RuleFor(query => query.Password).NotEmpty().NotNull();
        }
    }
}
