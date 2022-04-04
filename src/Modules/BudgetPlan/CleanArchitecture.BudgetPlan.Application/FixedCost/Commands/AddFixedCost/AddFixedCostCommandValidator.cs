using CleanArchitecture.BudgetPlan.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.BudgetPlan.Application.FixedCost.Commands.AddFixedCost
{
    public class AddFixedCostCommandValidator : AbstractValidator<AddFixedCostCommand>
    {
        private readonly IBudgetPlanDbContext dbContext;

        public AddFixedCostCommandValidator(IBudgetPlanDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.Name).NotNull().NotEmpty().MustAsync(HasUniqueName).WithMessage("Name must be unique.");
            RuleFor(c => c.Value).GreaterThan(0);
        }

        private async Task<bool> HasUniqueName(string name, CancellationToken cancellationToken)
        {
            return (await this.dbContext.FixedCost.FirstOrDefaultAsync(i => i.Name == name, cancellationToken)) is null;
        }
    }
}
