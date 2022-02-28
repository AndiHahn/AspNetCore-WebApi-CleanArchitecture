using System;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Application;
using CleanArchitecture.Shared.Core.Filter;
using CleanArchitecture.Shared.Core.Models;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Api.Dtos;
using CleanArchitecture.Shopping.Application.Bill;
using CleanArchitecture.Shopping.Application.Bill.Commands;
using CleanArchitecture.Shopping.Application.Bill.Queries;
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
            currentUserId = httpContextAccessor.HttpContext.User.GetUserId();
            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<BillDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public Task<PagedResult<BillDto>> List([FromQuery] SearchBillsParameters parameters)
            => sender.Send(
                new SearchBillsQuery(
                    this.currentUserId,
                    parameters.PageSize,
                    parameters.PageIndex,
                    parameters.IncludeShared,
                    parameters.Search));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BillDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<Result<BillDto>> GetById(Guid id)
            => sender.Send(new GetBillByIdQuery(this.currentUserId, id));

        [HttpPost]
        [ProducesResponseType(typeof(BillDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateBill([FromBody] BillCreateDto dto)
        {
            var bill =  await this.sender.Send(new CreateBillCommand(
                this.currentUserId,
                dto.BankAccountId,
                dto.ShopName,
                dto.Price,
                dto.Category.FromDto(),
                dto.Date,
                dto.Notes));

            if (bill.Status != ResultStatus.Success) return this.ToActionResult(bill);

            return Created($"{HttpContext.Request.Path}/{bill.Value.Id}", bill.Value);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BillDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<Result<BillDto>> UpdateBill(Guid id, [FromBody] BillUpdateDto dto)
            => this.sender.Send(
                new UpdateBillCommand(
                    this.currentUserId,
                    id,
                    dto.ShopName,
                    dto.Price,
                    dto.Date,
                    dto.Notes,
                    dto.Category?.FromDto(),
                    dto.Version));

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<Result> DeleteBill(Guid id)
            => this.sender.Send(new DeleteBillCommand(this.currentUserId, id));

        [HttpGet("{id}/image")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImage(Guid id)
        {
            var result = await this.sender.Send(new GetBillImageQuery(this.currentUserId, id));
            
            if (result.Status != ResultStatus.Success) return this.ToActionResult(result);

            return File(result.Value.Content, result.Value.ContentType);
        }

        [HttpPost("{id}/image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<Result> AddImage(Guid id, [FromForm] IFormFile image)
            => this.sender.Send(new AddImageToBillCommand(this.currentUserId, id, image));

        [HttpDelete("{id}/image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<Result> DeleteImage(Guid id)
            => this.sender.Send(new DeleteImageFromBillCommand(this.currentUserId, id));
    }
}