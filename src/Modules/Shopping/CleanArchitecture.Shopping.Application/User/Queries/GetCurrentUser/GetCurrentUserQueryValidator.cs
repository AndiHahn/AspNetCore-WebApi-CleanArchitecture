using FluentValidation;

namespace CleanArchitecture.Shopping.Application.User.Queries.GetCurrentUser
{
    public class GetCurrentUserQueryValidator : AbstractValidator<GetCurrentUserQuery>
    {
        public GetCurrentUserQueryValidator()
        {
            RuleFor(query => query.CurrentUserId).NotEmpty();
        }
    }
}
