using CleanArchitecture.BudgetPlan.Core;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Models.Result;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.BudgetPlan.Application.BudgetPlan.Queries
{
    public class GetBudgetPlanQuery : IQuery<Result<BudgetPlanDto>>
    {
        public GetBudgetPlanQuery(Guid userId)
        {
            this.UserId = userId;
        }

        public Guid UserId { get; }
    }

    internal class GetBudgetPlanQueryHandler : IQueryHandler<GetBudgetPlanQuery, Result<BudgetPlanDto>>
    {
        private readonly IBudgetPlanDbContext dbContext;

        public GetBudgetPlanQueryHandler(IBudgetPlanDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result<BudgetPlanDto>> Handle(GetBudgetPlanQuery request, CancellationToken cancellationToken)
        {
            var incomes = await this.dbContext.Income
                .Where(i => i.UserId == request.UserId)
                .ToListAsync(cancellationToken);
            var fixedCosts = await this.dbContext.FixedCost
                .Where(c => c.UserId == request.UserId)
                .ToListAsync(cancellationToken);
            var costsByGroup = fixedCosts.GroupBy(c => c.Category);

            return Result<BudgetPlanDto>.Success(new BudgetPlanDto
            {
                Incomes = incomes.Select(i => new BudgetPlanIncomeDto
                {
                    Name = i.Name,
                    Value = ToTwoDecimals(i.Value)
                }),
                Costs = costsByGroup
                    .Select(c => new BudgetPlanCostDto
                    {
                        Category = c.Key.FromCategory(),
                        Value = c.Sum(v => ToTwoDecimals(v.GetMonthlyValue()))
                    })
                    .Append(new BudgetPlanCostDto
                    {
                        Category = BudgetPlanCategoryDto.LivingCosts,
                        Value = GetLivingCosts(incomes.Sum(i => i.Value))
                    })
            });
        }

        private static double GetLivingCosts(double fullIncome) => fullIncome * 0.35;

        private static double ToTwoDecimals(double value) => Math.Truncate(value * 100) / 100;
    }
}
