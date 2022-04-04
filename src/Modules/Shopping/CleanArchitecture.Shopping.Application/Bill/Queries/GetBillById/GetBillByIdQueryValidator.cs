using FluentValidation;

namespace CleanArchitecture.Shopping.Application.Bill.Queries.GetBillById
{
    public class GetBillByIdQueryValidator : AbstractValidator<GetBillByIdQuery>
    {
        public GetBillByIdQueryValidator()
        {
            RuleFor(query => query.BillId).NotEmpty();
            RuleFor(query => query.CurrentUserId).NotEmpty();
        }
    }
}
