using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.BudgetPlan.Core;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.BudgetPlan.Application.FixedCost.Queries.ListFixedCosts
{
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
            return await this.dbContext.FixedCost
                .Where(i => i.UserId == request.UserId)
                .ProjectTo<FixedCostDto>(this.mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
