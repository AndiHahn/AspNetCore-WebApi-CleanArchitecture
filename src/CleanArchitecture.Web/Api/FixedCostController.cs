using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Services.FixedCost;
using CleanArchitecture.Core.Interfaces.Services.FixedCost.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Web.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FixedCostController : ControllerBase
    {
        private readonly IFixedCostService fixedCostService;
        private readonly ILogger<FixedCostController> logger;

        public FixedCostController(IFixedCostService fixedCostService, ILogger<FixedCostController> logger)
        {
            this.fixedCostService = fixedCostService ?? throw new ArgumentNullException(nameof(fixedCostService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("account")]
        [ProducesResponseType(typeof(IEnumerable<FixedCostModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFixedCostsByAccountIds([FromQuery] IEnumerable<int> accounts)
        {
            logger.LogInformation("Get Incomes by accounts {accountIds}", accounts);
            return Ok(await fixedCostService.GetByAccountIdsAsync(accounts));
        }

        [HttpPost]
        [ProducesResponseType(typeof(FixedCostModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddFixedCost([FromBody] FixedCostCreateModel createModel)
        {
            logger.LogInformation("Add fixed cost {model}", createModel);
            var createdIncome = await fixedCostService.AddFixedCostAsync(createModel);
            return Created($"{HttpContext.Request.Path}/{createdIncome.Id}", createdIncome);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFixedCost(int id)
        {
            logger.LogInformation("Delete fixed cost {id}", id);
            await fixedCostService.DeleteFixedCostAsync(id);
            return NoContent();
        }
    }
}
