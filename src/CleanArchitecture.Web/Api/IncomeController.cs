using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Services.Income;
using CleanArchitecture.Services.Income.Models;

namespace CleanArchitecture.Web.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IncomeController : ControllerBase
    {
        private readonly IIncomeService incomeService;
        private readonly ILogger<IncomeController> logger;

        public IncomeController(IIncomeService incomeService, ILogger<IncomeController> logger)
        {
            this.incomeService = incomeService ?? throw new ArgumentNullException(nameof(incomeService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("account")]
        [ProducesResponseType(typeof(IEnumerable<IncomeModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetIncomesByAccountIds([FromQuery] IEnumerable<int> accounts)
        {
            logger.LogInformation("Get incomes by account ids {accounts}", accounts);
            return Ok(await incomeService.GetByAccountIdsAsync(accounts));
        }

        [HttpPost]
        [ProducesResponseType(typeof(IncomeModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddIncome([FromBody] IncomeCreateModel createModel)
        {
            logger.LogInformation("Add income {model}", createModel);
            var createdIncome = await incomeService.AddIncomeAsync(createModel);
            return Created($"{HttpContext.Request.Path}/{createdIncome.Id}", createdIncome);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteIncome(int id)
        {
            logger.LogInformation("Delete income {id}", id);
            await incomeService.DeleteIncomeAsync(id);
            return NoContent();
        }
    }
}