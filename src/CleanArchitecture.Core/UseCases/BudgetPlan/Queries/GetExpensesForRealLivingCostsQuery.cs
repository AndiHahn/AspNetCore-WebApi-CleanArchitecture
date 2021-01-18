using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Infrastructure;
using CleanArchitecture.Core.Interfaces.Services.BudgetPlan.Models;
using CleanArchitecture.Core.Interfaces.Services.FixedCost;
using CleanArchitecture.Core.UseCases.Expenses.Queries;
using CleanArchitecture.Domain.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Core.UseCases.BudgetPlan.Queries
{
    public class GetExpensesForRealLivingCostsQuery : IRequest<IEnumerable<BudgetPlanModel>>
    {
        public IEnumerable<Guid> AccountIds { get; }
        public DateTime FromDate { get; }
        public DateTime ToDate { get; }

        public GetExpensesForRealLivingCostsQuery(
            IEnumerable<Guid> accountIds,
            DateTime fromDate,
            DateTime toDate)
        {
            AccountIds = accountIds;
            FromDate = fromDate;
            ToDate = toDate;
        }
    }

    public class GetExpensesForRealLivingCostsQueryHandler : IRequestHandler<GetExpensesForRealLivingCostsQuery, IEnumerable<BudgetPlanModel>>
    {
        private readonly IBudgetContext context;
        private readonly IFixedCostService fixedCostService;
        private readonly IMediator mediator;
        private readonly ILogger<GetExpensesForRealLivingCostsQueryHandler> logger;

        public GetExpensesForRealLivingCostsQueryHandler(
            IBudgetContext context,
            IFixedCostService fixedCostService,
            IMediator mediator,
            ILogger<GetExpensesForRealLivingCostsQueryHandler> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.fixedCostService = fixedCostService ?? throw new ArgumentNullException(nameof(fixedCostService));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<BudgetPlanModel>> Handle(
            GetExpensesForRealLivingCostsQuery request,
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

            budgetExpenses.Add(await GetRealLivingCostsAsync(
                request.AccountIds,
                request.FromDate,
                request.ToDate));

            return budgetExpenses;
        }

        public async Task<BudgetPlanModel> GetRealLivingCostsAsync(
            IEnumerable<Guid> accounts,
            DateTime fromDate,
            DateTime toDate)
        {
            var expenses = await mediator.Send(new GetExpensesInTimeRangeQuery(accounts, fromDate, toDate));
            double livingCosts = expenses.Sum(e => e.Costs).ToTwoDecimals();

            logger.LogInformation("Real living costs: {livingCosts}", livingCosts);

            return new BudgetPlanModel
            {
                Name = "Living costs",
                Value = livingCosts
            };
        }
    }
}
