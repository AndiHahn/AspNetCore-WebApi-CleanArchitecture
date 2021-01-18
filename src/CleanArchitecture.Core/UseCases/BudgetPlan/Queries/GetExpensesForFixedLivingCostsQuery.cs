using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Infrastructure;
using CleanArchitecture.Core.Interfaces.Services.BudgetPlan.Models;
using CleanArchitecture.Core.Interfaces.Services.FixedCost;
using CleanArchitecture.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Core.UseCases.BudgetPlan.Queries
{
    public class GetExpensesForFixedLivingCostsQuery : IRequest<IEnumerable<BudgetPlanModel>>
    {
        public IEnumerable<Guid> AccountIds { get; }

        public GetExpensesForFixedLivingCostsQuery(IEnumerable<Guid> accountIds)
        {
            AccountIds = accountIds;
        }
    }

    public class
        GetExpensesForFixedLivingCostsQueryHandler : IRequestHandler<GetExpensesForFixedLivingCostsQuery,
            IEnumerable<BudgetPlanModel>>
    {
        private readonly IBudgetContext context;
        private readonly IFixedCostService fixedCostService;
        private readonly IMediator mediator;
        private readonly ILogger<GetExpensesForFixedLivingCostsQueryHandler> logger;

        public GetExpensesForFixedLivingCostsQueryHandler(
            IBudgetContext context,
            IFixedCostService fixedCostService,
            IMediator mediator,
            ILogger<GetExpensesForFixedLivingCostsQueryHandler> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.fixedCostService = fixedCostService ?? throw new ArgumentNullException(nameof(fixedCostService));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<BudgetPlanModel>> Handle(
            GetExpensesForFixedLivingCostsQuery request,
            CancellationToken cancellationToken)
        {
            var allAccounts = await context.Account.ToListAsync(cancellationToken);
            var expenses = await fixedCostService.GetByAccountIdsAsync(request.AccountIds);
            var expensesByAccount = expenses.GroupBy(i => i.CostCategory);

            var budgetExpenses = expensesByAccount.Select(e => new BudgetPlanModel
            {
                Name = e.Key.ToString(),
                Value = e.Sum(b => b.ToSharedValue(allAccounts, request.AccountIds.ToList()))
            }).ToList();

            budgetExpenses.Add(await GetCalculatedLivingCostsAsync(request.AccountIds));

            return budgetExpenses;
        }

        public async Task<BudgetPlanModel> GetCalculatedLivingCostsAsync(IEnumerable<Guid> accounts)
        {
            var incomes = await mediator.Send(new GetIncomesForAccountQuery(accounts));

            double livingCosts = (incomes.Sum(i => i.Value) * 0.35).ToTwoDecimals();

            logger.LogInformation("Living costs: {livingCosts}", livingCosts);

            return new BudgetPlanModel
            {
                Name = "Living costs (35%)",
                Value = livingCosts
            };
        }
    }
}
