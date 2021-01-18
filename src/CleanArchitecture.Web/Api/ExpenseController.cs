using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Services.Expense.Models;
using CleanArchitecture.Core.UseCases.Expenses.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Web.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<ExpenseController> logger;

        public ExpenseController(IMediator mediator, ILogger<ExpenseController> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("{fromDate}/{toDate}")]
        [ProducesResponseType(typeof(IEnumerable<ExpenseModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExpenseStatistics(DateTime fromDate, DateTime toDate, [FromQuery] IEnumerable<Guid> accounts)
        {
            logger.LogInformation("GetExpenseStatistics from {fromDate} to {toDate} for Accounts {accountIds}", fromDate, toDate, accounts);

            return Ok(await mediator.Send(new GetExpensesInTimeRangeQuery(accounts, fromDate, toDate)));
        }
    }
}
