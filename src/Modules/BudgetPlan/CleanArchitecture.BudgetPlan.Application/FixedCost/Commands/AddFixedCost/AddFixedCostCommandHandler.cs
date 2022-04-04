using AutoMapper;
using CleanArchitecture.BudgetPlan.Core;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.BudgetPlan.Application.FixedCost.Commands.AddFixedCost
{
    internal class AddFixedCostCommandHandler : ICommandHandler<AddFixedCostCommand, Result<FixedCostDto>>
    {
        private readonly IBudgetPlanDbContext dbContext;
        private readonly IMapper mapper;

        public AddFixedCostCommandHandler(
            IBudgetPlanDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Result<FixedCostDto>> Handle(AddFixedCostCommand request, CancellationToken cancellationToken)
        {
            var fixedCost = new Core.FixedCost(
                request.UserId,
                request.Name,
                request.Value,
                request.Duration,
                request.Category);

            await this.dbContext.FixedCost.AddAsync(fixedCost, cancellationToken);

            await this.dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<FixedCostDto>(fixedCost);
        }
    }
}
