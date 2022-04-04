using AutoMapper;
using CleanArchitecture.BudgetPlan.Core;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.BudgetPlan.Application.Income.Commands.AddIncome
{
    internal class AddIncomeCommandHandler : ICommandHandler<AddIncomeCommand, Result<IncomeDto>>
    {
        private readonly IBudgetPlanDbContext dbContext;
        private readonly IMapper mapper;

        public AddIncomeCommandHandler(
            IBudgetPlanDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<IncomeDto>> Handle(AddIncomeCommand request, CancellationToken cancellationToken)
        {
            var income = new Core.Income(request.UserId, request.Name, request.Value, request.Duration);

            await this.dbContext.Income.AddAsync(income, cancellationToken);

            await this.dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<IncomeDto>(income);
        }
    }
}
