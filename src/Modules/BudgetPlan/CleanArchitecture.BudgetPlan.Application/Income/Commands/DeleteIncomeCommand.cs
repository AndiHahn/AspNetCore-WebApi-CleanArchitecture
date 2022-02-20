﻿using CleanArchitecture.BudgetPlan.Core;
using CleanArchitecture.Shared.Core.Models.Result;
using MediatR;

namespace CleanArchitecture.BudgetPlan.Application.Income.Commands
{
    public class DeleteIncomeCommand : IRequest<Result>
    {
        public DeleteIncomeCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }

    internal class DeleteIncomeCommandHandler : IRequestHandler<DeleteIncomeCommand, Result>
    {
        private readonly IBudgetPlanDbContext dbContext;

        public DeleteIncomeCommandHandler(
            IBudgetPlanDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result> Handle(DeleteIncomeCommand request, CancellationToken cancellationToken)
        {
            var income = await this.dbContext.Income.FindAsync(request.Id);
            if (income is null)
            {
                return Result.NotFound();
            }

            this.dbContext.Income.Remove(income);

            await this.dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
