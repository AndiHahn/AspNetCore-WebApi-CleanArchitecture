using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Services.BudgetPlan.Models;

namespace CleanArchitecture.Services.BudgetPlan
{
    public interface IBudgetPlanService
    {
        Task<IEnumerable<BudgetPlanModel>> GetIncomesAsync(IEnumerable<int> accountIds);
        Task<IEnumerable<BudgetPlanModel>> GetExpensesAsync(IEnumerable<int> accountIds, bool realLivingCosts = false, DateTime fromDate = default, DateTime toDate = default);
    }
}
