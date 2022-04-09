using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Application;
using CleanArchitecture.Shared.Core.Filter;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Api.Dtos;
using CleanArchitecture.Shopping.Application.Bill;
using CleanArchitecture.Shopping.Application.Bill.Commands.AddImageToBill;
using CleanArchitecture.Shopping.Application.Bill.Commands.CreateBill;
using CleanArchitecture.Shopping.Application.Bill.Commands.DeleteBill;
using CleanArchitecture.Shopping.Application.Bill.Commands.DeleteImageFromBill;
using CleanArchitecture.Shopping.Application.Bill.Commands.UpdateBill;
using CleanArchitecture.Shopping.Application.Bill.Queries.GetBillById;
using CleanArchitecture.Shopping.Application.Bill.Queries.GetBillImage;
using CleanArchitecture.Shopping.Application.Bill.Queries.SearchBills;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Shopping.Api.Controllers
{
    [MapToProblemDetails]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly ISender sender;
        private readonly Guid currentUserId;

        public BillController(
            ISender sender,
            IHttpContextAccessor httpContextAccessor)
        {
            currentUserId = httpContextAccessor.HttpContext?.User.GetUserId() ?? Guid.Empty;
            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<BillDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public Task<PagedResult<BillDto>> List(
            [FromQuery] SearchBillsParameters parameters,
            CancellationToken cancellationToken)
            => sender.Send(
                new SearchBillsQuery(
                    this.currentUserId,
                    parameters.PageSize,
                    parameters.PageIndex,
                    parameters.IncludeShared,
                    parameters.Search),
                cancellationToken);

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BillDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<Result<BillDto>> GetById(Guid id, CancellationToken cancellationToken)
            => sender.Send(new GetBillByIdQuery(this.currentUserId, id), cancellationToken);

        [HttpPost]
        [ProducesResponseType(typeof(BillDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateBill([FromBody] BillCreateDto dto, CancellationToken cancellationToken)
        {
            var bill =  await this.sender.Send(new CreateBillCommand(
                this.currentUserId,
                dto.BankAccountId,
                dto.ShopName,
                dto.Price,
                dto.Category.FromDto(),
                dto.Date,
                dto.Notes),
                cancellationToken);

            if (bill.Status != ResultStatus.Success) return this.ToActionResult(bill);

            return Created($"{HttpContext.Request.Path}/{bill.Value?.Id}", bill.Value);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BillDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<Result<BillDto>> UpdateBill(
            Guid id,
            [FromBody] BillUpdateDto dto,
            CancellationToken cancellationToken)
            => this.sender.Send(
                new UpdateBillCommand(
                    this.currentUserId,
                    id,
                    dto.ShopName,
                    dto.Price,
                    dto.Date,
                    dto.Notes,
                    dto.Category?.FromDto(),
                    dto.Version),
                cancellationToken);

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<Result> DeleteBill(Guid id, CancellationToken cancellationToken)
            => this.sender.Send(new DeleteBillCommand(this.currentUserId, id), cancellationToken);

        [HttpGet("{id}/image")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImage(Guid id, CancellationToken cancellationToken)
        {
            var result = await this.sender.Send(new GetBillImageQuery(this.currentUserId, id), cancellationToken);
            
            if (result.Status != ResultStatus.Success) return this.ToActionResult(result);

            return File(result.Value.Content, result.Value.ContentType);
        }

        [HttpPost("{id}/image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<Result> AddImage(Guid id, [FromForm] IFormFile image, CancellationToken cancellationToken)
            => this.sender.Send(new AddImageToBillCommand(this.currentUserId, id, image), cancellationToken);

        [HttpDelete("{id}/image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<Result> DeleteImage(Guid id, CancellationToken cancellationToken)
            => this.sender.Send(new DeleteImageFromBillCommand(this.currentUserId, id), cancellationToken);
    }
}