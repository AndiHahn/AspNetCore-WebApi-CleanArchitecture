using CleanArchitecture.BudgetPlan.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.BudgetPlan.Application.Income.Commands.AddIncome
{
    public class AddIncomeCommandValidator : AbstractValidator<AddIncomeCommand>
    {
        private readonly IBudgetPlanDbContext dbContext;

        public AddIncomeCommandValidator(IBudgetPlanDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

            RuleFor(c => c.UserId).NotEmpty();
            RuleFor(c => c.Name).NotNull().NotEmpty().MustAsync(HasUniqueName).WithMessage("Name must be unique.");
            RuleFor(c => c.Value).GreaterThan(0);
        }

        private async Task<bool> HasUniqueName(string name, CancellationToken cancellationToken)
            => (await this.dbContext.Income.FirstOrDefaultAsync(i => i.Name == name, cancellationToken)) is null;
    }
}
