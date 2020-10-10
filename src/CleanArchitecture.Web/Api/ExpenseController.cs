using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Services.Expense;
using CleanArchitecture.Services.Expense.Models;

namespace CleanArchitecture.Web.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService expenseService;
        private readonly ILogger<ExpenseController> logger;

        public ExpenseController(IExpenseService expenseService, ILogger<ExpenseController> logger)
        {
            this.expenseService = expenseService ?? throw new ArgumentNullException(nameof(expenseService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{fromDate}/{toDate}")]
        [ProducesResponseType(typeof(IEnumerable<ExpenseModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExpenseStatistics(DateTime fromDate, DateTime toDate, [FromQuery] IEnumerable<int> accounts)
        {
            logger.LogInformation("GetExpenseStatistics from {fromDate} to {toDate} for Accounts {accountIds}", fromDate, toDate, accounts);
            return Ok(await expenseService.GetExpensesAsync(accounts, fromDate, toDate));
        }
    }
}
