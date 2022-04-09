using CleanArchitecture.BudgetPlan.Api.Dtos;
using CleanArchitecture.BudgetPlan.Application;
using CleanArchitecture.BudgetPlan.Application.Income;
using CleanArchitecture.BudgetPlan.Application.Income.Commands.AddIncome;
using CleanArchitecture.BudgetPlan.Application.Income.Commands.DeleteIncome;
using CleanArchitecture.BudgetPlan.Application.Income.Queries.ListIncomes;
using CleanArchitecture.Shared.Application;
using CleanArchitecture.Shared.Core.Filter;
using CleanArchitecture.Shared.Core.Result;
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
    public class IncomeController : ControllerBase
    {
        private readonly ISender sender;
        private readonly Guid currentUserId;

        public IncomeController(
            ISender sender,
            IHttpContextAccessor httpContextAccessor)
        {
            currentUserId = httpContextAccessor.HttpContext?.User.GetUserId() ?? Guid.Empty;
            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IncomeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public Task<Result<IEnumerable<IncomeDto>>> ListIncomes(CancellationToken cancellationToken)
            => this.sender.Send(new ListIncomesQuery(currentUserId), cancellationToken);

        [HttpPost]
        [ProducesResponseType(typeof(IncomeDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddIncome([FromBody] IncomeCreateDto dto, CancellationToken cancellationToken)
        {
            var income = await this.sender.Send(new AddIncomeCommand(
                this.currentUserId,
                dto.Name,
                dto.Value,
                dto.Duration.FromDto()),
                cancellationToken);

            if (income.Status != ResultStatus.Success) return this.ToActionResult(income);

            return Created($"{HttpContext.Request.Path}/{income.Value?.Id}", income.Value);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<Result> DeleteIncome(Guid id, CancellationToken cancellationToken)
            => this.sender.Send(new DeleteIncomeCommand(id), cancellationToken);
    }
}
