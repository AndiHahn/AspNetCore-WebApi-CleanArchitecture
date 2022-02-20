using CleanArchitecture.BudgetPlan.Core;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Models.Result;

namespace CleanArchitecture.BudgetPlan.Application.FixedCost.Commands
{
    public class DeleteFixedCostCommand : ICommand<Result>
    {
        public DeleteFixedCostCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }

    internal class DeleteFixedCostCommandHandler : ICommandHandler<DeleteFixedCostCommand, Result>
    {
        private readonly IBudgetPlanDbContext dbContext;

        public DeleteFixedCostCommandHandler(IBudgetPlanDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result> Handle(DeleteFixedCostCommand request, CancellationToken cancellationToken)
        {
            var fixedCost = await dbContext.FixedCost.FindAsync(request.Id, cancellationToken);
            if (fixedCost is null)
            {
                return Result.NotFound();
            }

            this.dbContext.FixedCost.Remove(fixedCost);

            await this.dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
