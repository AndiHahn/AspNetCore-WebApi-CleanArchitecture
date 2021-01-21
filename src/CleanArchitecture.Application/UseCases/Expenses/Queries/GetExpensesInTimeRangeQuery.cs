using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.UseCases.Expenses.Models;
using CleanArchitecture.Application.Validations;
using CleanArchitecture.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.UseCases.Expenses.Queries
{
    public class GetExpensesInTimeRangeQuery : IRequest<IEnumerable<ExpenseModel>>
    {
        public IEnumerable<Guid> Accounts { get; }
        public DateTime FromDate { get; }
        public DateTime ToDate { get; }

        public GetExpensesInTimeRangeQuery(
            IEnumerable<Guid> accounts,
            DateTime fromDate,
            DateTime toDate)
        {
            Accounts = accounts;
            FromDate = fromDate;
            ToDate = toDate;
        }
    }

    public class GetExpensesInTimeRangeQueryHandler : IRequestHandler<GetExpensesInTimeRangeQuery, IEnumerable<ExpenseModel>>
    {
        private readonly IBudgetContext context;
        private readonly ILogger<GetExpensesInTimeRangeQueryHandler> logger;

        public GetExpensesInTimeRangeQueryHandler(
            IBudgetContext context,
            ILogger<GetExpensesInTimeRangeQueryHandler> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ExpenseModel>> Handle(
            GetExpensesInTimeRangeQuery request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Get Expenses...");
            var results = new List<IEnumerable<ExpenseModel>>();
            foreach (var accountId in request.Accounts)
            {
                logger.LogInformation("Get Expense by account {accountId}", accountId);
                results.Add(await GetExpensesByAccountAsync(accountId, request.FromDate, request.ToDate));
            }
            logger.LogInformation("Aggregate results...");
            return AggregateResults(results);
        }

        private async Task<IEnumerable<ExpenseModel>> GetExpensesByAccountAsync(Guid accountId, DateTime fromDate, DateTime toDate)
        {
            (await context.Account.FindAsync(accountId)).AssertEntityFound(accountId);

            return await context.Bill
                .Include(b => b.BillCategory)
                .Where(b => b.AccountId == accountId &&
                            b.Date >= fromDate && b.Date <= toDate)
                .GroupBy(b => b.BillCategory.Name)
                .Select(g => new ExpenseModel(g.Key, g.Sum(b => b.Price)))
                .ToListAsync();
        }

        private IEnumerable<ExpenseModel> AggregateResults(IList<IEnumerable<ExpenseModel>> result)
        {
            Dictionary<string, IEnumerable<double>> aggregatDict = new Dictionary<string, IEnumerable<double>>();
            foreach (var res in result)
            {
                foreach (var item in res)
                {
                    if (aggregatDict.ContainsKey(item.Category))
                    {
                        aggregatDict[item.Category] = aggregatDict[item.Category].Append(item.Costs);
                    }
                    else
                    {
                        aggregatDict.Add(item.Category, new List<double> { item.Costs });
                    }
                }
            }

            return aggregatDict.Select(e => new ExpenseModel(e.Key, e.Value.Sum()))
                               .OrderByDescending(e => e.Costs);
        }
    }
}
