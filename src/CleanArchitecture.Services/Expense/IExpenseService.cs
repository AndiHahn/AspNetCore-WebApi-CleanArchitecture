using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Services.Expense.Models;

namespace CleanArchitecture.Services.Expense
{
    public interface IExpenseService
    {
        Task<IEnumerable<ExpenseModel>> GetExpensesAsync(IEnumerable<int> accounts, DateTime fromDate, DateTime toDate);
    }
}
