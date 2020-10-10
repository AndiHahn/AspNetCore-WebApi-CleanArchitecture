using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Services.BudgetPlan;
using CleanArchitecture.Services.BudgetPlan.Models;

namespace CleanArchitecture.Web.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetPlanController : ControllerBase
    {
        private readonly IBudgetPlanService budgetPlanService;

        public BudgetPlanController(IBudgetPlanService budgetPlanService)
        {
            this.budgetPlanService = budgetPlanService ?? throw new ArgumentNullException(nameof(budgetPlanService));
        }

        [HttpGet("income")]
        [ProducesResponseType(typeof(IEnumerable<BudgetPlanModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetIncomes([FromQuery] IEnumerable<int> accounts)
        {
            return Ok(await budgetPlanService.GetIncomesAsync(accounts));
        }

        [HttpGet("expense")]
        [ProducesResponseType(typeof(IEnumerable<BudgetPlanModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetExpenses([FromQuery] IEnumerable<int> accounts)
        {
            return Ok(await budgetPlanService.GetExpensesAsync(accounts));
        }

        [HttpGet("expense/real/{fromDate}/{toDate}")]
        [ProducesResponseType(typeof(IEnumerable<BudgetPlanModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetRealExpenses(DateTime fromDate, DateTime toDate, [FromQuery] IEnumerable<int> accounts)
        {
            return Ok(await budgetPlanService.GetExpensesAsync(accounts, true, fromDate, toDate));
        }
    }
}
