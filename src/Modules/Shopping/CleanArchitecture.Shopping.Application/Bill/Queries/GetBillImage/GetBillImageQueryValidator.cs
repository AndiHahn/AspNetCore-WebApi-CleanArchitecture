using FluentValidation;

namespace CleanArchitecture.Shopping.Application.Bill.Queries.GetBillImage
{
    public class GetBillImageQueryValidator : AbstractValidator<GetBillImageQuery>
    {
        public GetBillImageQueryValidator()
        {
            RuleFor(query => query.BillId).NotEmpty();
            RuleFor(query => query.CurrentUserId).NotEmpty();
        }
    }
}
