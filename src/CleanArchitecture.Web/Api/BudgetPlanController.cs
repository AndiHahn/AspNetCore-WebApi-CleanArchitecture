using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Services.BudgetPlan.Models;
using CleanArchitecture.Core.UseCases.BudgetPlan.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetPlanController : ControllerBase
    {
        private readonly IMediator mediator;

        public BudgetPlanController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("income")]
        [ProducesResponseType(typeof(IEnumerable<BudgetPlanModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetIncomes([FromQuery] IEnumerable<Guid> accounts)
        {
            return Ok(await mediator.Send(new GetIncomesForAccountQuery(accounts)));
        }

        [HttpGet("expense")]
        [ProducesResponseType(typeof(IEnumerable<BudgetPlanModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetExpenses([FromQuery] IEnumerable<Guid> accounts)
        {
            return Ok(await mediator.Send(new GetExpensesForFixedLivingCostsQuery(accounts)));
        }

        [HttpGet("expense/real/{fromDate}/{toDate}")]
        [ProducesResponseType(typeof(IEnumerable<BudgetPlanModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetRealExpenses(
            DateTime fromDate,
            DateTime toDate,
            [FromQuery] IEnumerable<Guid> accounts)
        {
            return Ok(await mediator.Send(new GetExpensesForRealLivingCostsQuery(accounts, fromDate, toDate)));
        }
    }
}
