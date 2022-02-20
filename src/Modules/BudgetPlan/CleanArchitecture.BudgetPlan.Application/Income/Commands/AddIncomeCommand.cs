using AutoMapper;
using CleanArchitecture.BudgetPlan.Core;
using CleanArchitecture.Shared.Core.Models.Result;
using MediatR;

namespace CleanArchitecture.BudgetPlan.Application.Income.Commands
{
    public class AddIncomeCommand : IRequest<Result<IncomeDto>>
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

    internal class AddIncomeCommandHandler : IRequestHandler<AddIncomeCommand, Result<IncomeDto>>
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
