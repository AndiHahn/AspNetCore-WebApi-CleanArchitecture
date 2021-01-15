using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Infrastructure;
using CleanArchitecture.Core.Interfaces.Services.BudgetPlan;
using CleanArchitecture.Core.Interfaces.Services.BudgetPlan.Models;
using CleanArchitecture.Core.Interfaces.Services.Expense;
using CleanArchitecture.Core.Interfaces.Services.FixedCost;
using CleanArchitecture.Core.Interfaces.Services.FixedCost.Models;
using CleanArchitecture.Core.Interfaces.Services.Income;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Core.UseCases.BudgetPlan
{
    public class BudgetPlanService : IBudgetPlanService
    {
        private readonly IBudgetContext context;
        private readonly IIncomeService incomeService;
        private readonly IFixedCostService fixedCostService;
        private readonly IExpenseService expenseService;
        private readonly ILogger<BudgetPlanService> logger;

        public BudgetPlanService(IBudgetContext context,
                                 IIncomeService incomeService,
                                 IFixedCostService fixedCostService,
                                 IExpenseService expenseService,
                                 ILogger<BudgetPlanService> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.incomeService = incomeService ?? throw new ArgumentNullException(nameof(incomeService));
            this.fixedCostService = fixedCostService ?? throw new ArgumentNullException(nameof(fixedCostService));
            this.expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<BudgetPlanModel>> GetIncomesAsync(IEnumerable<int> accountIds)
        {
            var incomes = await incomeService.GetByAccountIdsAsync(accountIds);
            var incomesByAccount = incomes.GroupBy(i => i.AccountName);
            return incomesByAccount.Select(a => new BudgetPlanModel()
            {
                Name = a.Key,
                Value = a.Sum(b => ToMonthlyValue(b.Duration, b.Value))
            });
        }

        public async Task<IEnumerable<BudgetPlanModel>> GetExpensesAsync(IEnumerable<int> accountIds, bool realLivingCosts = false, DateTime fromDate = default, DateTime toDate = default)
        {
            var allAccounts = await context.Account.ToListAsync();
            var expenses = await fixedCostService.GetByAccountIdsAsync(accountIds);
            var expensesByAccount = expenses.GroupBy(i => i.CostCategory);
            var budgetExpenses = expensesByAccount.Select(e => new BudgetPlanModel()
            {
                Name = e.Key.ToString(),
                Value = e.Sum(b => ToSharedValue(allAccounts, accountIds.ToList(), b))
            }).ToList();
            budgetExpenses.Add(await GetLivingCostsAsync(realLivingCosts, accountIds, fromDate, toDate));
            return budgetExpenses;
        }

        private double ToSharedValue(IList<AccountEntity> allAccounts, IList<int> selectedAccountIds, FixedCostModel b)
        {
            AccountEntity sharedAccount = allAccounts.FirstOrDefault(a => a.Name.Contains("Gemeinsam"));

            //if not shared account -> return full value
            if (b.AccountName != sharedAccount.Name)
            {
                return ToMonthlyValue(b.Duration, b.Value);
            }
            else
            {
                //if all accounts are selected -> return full value
                if (allAccounts.Count == selectedAccountIds.Count)
                {
                    return ToMonthlyValue(b.Duration, b.Value);
                }
                //if only shared account selected -> return full value
                else if (selectedAccountIds.Count == 1 && selectedAccountIds.First() == sharedAccount.Id)
                {
                    return ToMonthlyValue(b.Duration, b.Value);
                }
                //if any account and shared account selected -> return half value
                else
                {
                    return ToTwoDecimals(ToMonthlyValue(b.Duration, b.Value) / 2);
                }
            }
        }

        public async Task<BudgetPlanModel> GetLivingCostsAsync(bool realLivingCosts, IEnumerable<int> accounts, DateTime fromDate, DateTime toDate)
        {
            if (realLivingCosts)
            {
                return await GetRealLivingCostsAsync(accounts, fromDate, toDate);
            }
            else
            {
                return await GetCalculatedLivingCostsAsync(accounts);
            }
        }

        public async Task<BudgetPlanModel> GetRealLivingCostsAsync(IEnumerable<int> accounts, DateTime fromDate, DateTime toDate)
        {
            var expenses = await expenseService.GetExpensesAsync(accounts, fromDate, toDate);
            double livingCosts = ToTwoDecimals(expenses.Sum(e => e.Costs));
            logger.LogInformation("Real living costs: {livingCosts}", livingCosts);
            return new BudgetPlanModel()
            {
                Name = "Living costs",
                Value = livingCosts
            };
        }

        public async Task<BudgetPlanModel> GetCalculatedLivingCostsAsync(IEnumerable<int> accounts)
        {
            var incomes = await GetIncomesAsync(accounts);
            double livingCosts = ToTwoDecimals(incomes.Sum(i => i.Value) * 0.35);
            logger.LogInformation("Living costs: {livingCosts}", livingCosts);
            return new BudgetPlanModel()
            {
                Name = "Living costs (35%)",
                Value = livingCosts
            };
        }

        private double ToMonthlyValue(Duration duration, double value)
        {
            double monthlyValue = 0;
            if (duration == Duration.Monthly)
            {
                monthlyValue = value;
            }
            else if (duration == Duration.QuarterYear)
            {
                monthlyValue = value * 4 / 12;
            }
            else if (duration == Duration.HalfYear)
            {
                monthlyValue = value * 2 / 12;
            }
            else if (duration == Duration.Year)
            {
                monthlyValue = value / 12;
            }

            return ToTwoDecimals(monthlyValue);
        }

        private double ToTwoDecimals(double value)
        {
            return Math.Truncate(value * 100) / 100;
        }
    }
}
