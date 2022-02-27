using AutoMapper;
using CleanArchitecture.BudgetPlan.Core;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.BudgetPlan.Application.FixedCost.Commands
{
    public class AddFixedCostCommand : ICommand<Result<FixedCostDto>>
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
