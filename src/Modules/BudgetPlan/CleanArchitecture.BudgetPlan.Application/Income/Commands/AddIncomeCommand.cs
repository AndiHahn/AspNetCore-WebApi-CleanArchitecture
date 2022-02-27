using AutoMapper;
using CleanArchitecture.BudgetPlan.Core;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.BudgetPlan.Application.Income.Commands
{
    public class AddIncomeCommand : ICommand<Result<IncomeDto>>
    {
        public AddIncomeCommand(Guid userId, string name, double value, Duration duration)
        {
            UserId = userId;
            Name = name;
            Value = value;
            Duration = duration;
        }

        public Guid UserId { get; }

        public string Name { get; }

        public double Value { get; }

        public Duration Duration { get; }
    }

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
