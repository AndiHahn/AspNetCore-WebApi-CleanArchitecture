using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Infrastructure;
using CleanArchitecture.Core.Interfaces.Services.Expense;
using CleanArchitecture.Core.Interfaces.Services.Expense.Models;
using CleanArchitecture.Core.Validations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Core.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IBudgetContext context;
        private readonly ILogger<ExpenseService> logger;

        public ExpenseService(IBudgetContext context, ILogger<ExpenseService> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ExpenseModel>> GetExpensesAsync(IEnumerable<int> accounts, DateTime fromDate, DateTime toDate)
        {
            logger.LogInformation("Get Expenses...");
            var results = new List<IEnumerable<ExpenseModel>>();
            foreach (var accountId in accounts)
            {
                logger.LogInformation("Get Expense by account {accountId}", accountId);
                results.Add(await GetExpensesByAccountAsync(accountId, fromDate, toDate));
            }
            logger.LogInformation("Aggregate results...");
            return AggregateResults(results);
        }

        private async Task<IEnumerable<ExpenseModel>> GetExpensesByAccountAsync(int accountId, DateTime fromDate, DateTime toDate)
        {
            (await context.Account.FindAsync(accountId))
                                  .AssertEntityFound(accountId);
            return await context.BillQueries.WithCategoryByAccountIdBetweenDate(accountId, fromDate, toDate)
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
                        aggregatDict.Add(item.Category, new List<double>() { item.Costs });
                    }
                }
            }
            return aggregatDict.Select(e => new ExpenseModel(e.Key, e.Value.Sum()))
                               .OrderByDescending(e => e.Costs);
        }
    }
}