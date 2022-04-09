using CleanArchitecture.BudgetPlan.Application.BudgetPlan;
using CleanArchitecture.BudgetPlan.Application.BudgetPlan.Queries.GetBudgetPlan;
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
    public class BudgetPlanController : ControllerBase
    {
        private readonly ISender sender;
        private readonly Guid currentUserId;

        public BudgetPlanController(
            ISender sender,
            IHttpContextAccessor httpContextAccessor)
        {
            currentUserId = httpContextAccessor.HttpContext?.User.GetUserId() ?? Guid.Empty;
            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        [HttpGet]
        [ProducesResponseType(typeof(BudgetPlanDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public Task<Result<BudgetPlanDto>> GetBudgetPlan(CancellationToken cancellationToken)
            => this.sender.Send(new GetBudgetPlanQuery(currentUserId), cancellationToken);
    }
}
