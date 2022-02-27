using AutoMapper;
using CleanArchitecture.BudgetPlan.Core;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.BudgetPlan.Application.FixedCost.Queries
{
    public class ListFixedCostsQuery : IQuery<Result<IEnumerable<FixedCostDto>>>
    {
        public ListFixedCostsQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }

    internal class ListFixedCostsQueryHandler : IQueryHandler<ListFixedCostsQuery, Result<IEnumerable<FixedCostDto>>>
    {
        private readonly IBudgetPlanDbContext dbContext;
        private readonly IMapper mapper;

        public ListFixedCostsQueryHandler(
            IBudgetPlanDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<IEnumerable<FixedCostDto>>> Handle(ListFixedCostsQuery request, CancellationToken cancellationToken)
        {
            var incomes = await this.dbContext.FixedCost
                .Where(i => i.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<FixedCostDto>>.Success(incomes.Select(this.mapper.Map<FixedCostDto>));
        }
    }
}
