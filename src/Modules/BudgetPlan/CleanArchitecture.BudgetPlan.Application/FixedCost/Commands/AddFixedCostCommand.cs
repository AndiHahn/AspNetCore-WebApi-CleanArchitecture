using AutoMapper;
using CleanArchitecture.BudgetPlan.Core;
using CleanArchitecture.Shared.Core.Models.Result;
using MediatR;

namespace CleanArchitecture.BudgetPlan.Application.FixedCost.Commands
{
    public class AddFixedCostCommand : IRequest<Result<FixedCostDto>>
    {
        public AddFixedCostCommand(
            Guid userId,
            string name,
            double value,
            Duration duration,
            CostCategory category)
        {
            UserId = userId;
            Name = name;
            Value = value;
            Duration = duration;
            Category = category;
        }

        public Guid UserId { get; }

        public string Name { get; }

        public double Value { get; }

        public Duration Duration { get; }

        public CostCategory Category { get; }
    }

    internal class AddFixedCostCommandHandler : IRequestHandler<AddFixedCostCommand, Result<FixedCostDto>>
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
