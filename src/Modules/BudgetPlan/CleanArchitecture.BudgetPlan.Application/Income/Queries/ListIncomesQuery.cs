using AutoMapper;
using CleanArchitecture.BudgetPlan.Core;
using CleanArchitecture.Shared.Core.Models.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.BudgetPlan.Application.Income.Queries
{
    public class ListIncomesQuery : IRequest<Result<IEnumerable<IncomeDto>>>
    {
        public ListIncomesQuery(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }

    internal class ListIncomesQueryHandler : IRequestHandler<ListIncomesQuery, Result<IEnumerable<IncomeDto>>>
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
            var incomes = await this.dbContext.Income
                .Where(i => i.UserId == request.UserId)
                .ToListAsync(cancellationToken);

            return Result<IEnumerable<IncomeDto>>.Success(incomes.Select(this.mapper.Map<IncomeDto>));
        }
    }
}
