using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.BudgetPlan.Core;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.BudgetPlan.Application.Income.Queries.ListIncomes
{
    internal class ListIncomesQueryHandler : IQueryHandler<ListIncomesQuery, Result<IEnumerable<IncomeDto>>>
    {
        private readonly IBudgetPlanDbContext dbContext;
        private readonly IMapper mapper;

        public ListIncomesQueryHandler(
            IBudgetPlanDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<IEnumerable<IncomeDto>>> Handle(ListIncomesQuery request, CancellationToken cancellationToken)
        {
            return await this.dbContext.Income
                .Where(i => i.UserId == request.UserId)
                .ProjectTo<IncomeDto>(this.mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
