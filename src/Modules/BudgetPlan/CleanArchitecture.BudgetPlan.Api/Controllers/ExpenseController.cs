using CleanArchitecture.BudgetPlan.Api.Dtos;
using CleanArchitecture.BudgetPlan.Application;
using CleanArchitecture.BudgetPlan.Application.FixedCost;
using CleanArchitecture.BudgetPlan.Application.FixedCost.Commands;
using CleanArchitecture.BudgetPlan.Application.FixedCost.Queries;
using CleanArchitecture.Shared.Application;
using CleanArchitecture.Shared.Core;
using CleanArchitecture.Shared.Core.Filter;
using CleanArchitecture.Shared.Core.Models.Result;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.BudgetPlan.Api.Controllers
{
    [MapToProblemDetails]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ISender sender;
        private readonly Guid currentUserId;

        public ExpenseController(
            ISender sender,
            IHttpContextAccessor httpContextAccessor)
        {
            currentUserId = httpContextAccessor.HttpContext.User.GetUserId();
            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FixedCostDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public Task<Result<IEnumerable<FixedCostDto>>> ListFixedCosts()
            => this.sender.Send(new ListFixedCostsQuery(currentUserId));

        [HttpPost]
        [ProducesResponseType(typeof(FixedCostDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddFixedCost([FromBody] FixedCostCreateDto dto)
        {
            var income = await this.sender.Send(new AddFixedCostCommand(
                this.currentUserId,
                dto.Name,
                dto.Value,
                dto.Duration.FromDto(),
                dto.Category.FromDto()));

            if (income.Status != ResultStatus.Success) return this.ToActionResult(income);

            return Created($"{HttpContext.Request.Path}/{income.Value.Id}", income.Value);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<Result> DeleteFixedCost(Guid id)
            => this.sender.Send(new DeleteFixedCostCommand(id));
    }
}
