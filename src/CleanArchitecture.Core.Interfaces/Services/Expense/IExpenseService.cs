using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Services.Expense.Models;

namespace CleanArchitecture.Core.Interfaces.Services.Expense
{
    public interface IExpenseService
    {
        Task<IEnumerable<ExpenseModel>> GetExpensesAsync(IEnumerable<int> accounts, DateTime fromDate, DateTime toDate);
    }
}
