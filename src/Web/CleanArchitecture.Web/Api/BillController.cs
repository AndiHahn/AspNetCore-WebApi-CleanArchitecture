using System;
using System.Threading.Tasks;
using CleanArchitecture.Application.Bill;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Application.User;
using CleanArchitecture.Core.Models;
using CleanArchitecture.Web.Api.Filter;
using CleanArchitecture.Web.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Api.Api
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
        [ProducesResponseType(typeof(PagedResult<BillDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List([FromQuery] SearchBillsParameters parameters)
            => Ok(await sender.Send(
                new SearchBillsQuery(
                    this.currentUserId,
                    parameters.PageSize,
                    parameters.PageIndex,
                    parameters.IncludeShared,
                    parameters.Search)));

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
                dto.Category.ToClass(),
                dto.Date,
                dto.Notes));

            if (bill.Status != ResultStatus.Success) return this.ToActionResult(bill);

            return Created($"{HttpContext.Request.Path}/{bill.Value.Id}", bill.Value);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BillDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBill(Guid id, [FromBody] BillUpdateDto dto)
            => Ok(await this.sender.Send(
                new UpdateBillCommand(
                    this.currentUserId,
                    id,
                    dto.ShopName,
                    dto.Price,
                    dto.Date,
                    dto.Notes,
                    dto.Category?.ToClass())));

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBill(Guid id)
        {
            await this.sender.Send(new DeleteBillCommand(this.currentUserId, id));
            return NoContent();
        }

        [HttpGet("{id}/image")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImage(Guid id)
        {
            var image = await this.sender.Send(new GetBillImageQuery(this.currentUserId, id));
            return File(image.Content, image.ContentType);
        }

        [HttpPost("{id}/image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddImage(Guid id, [FromForm] IFormFile image)
        {
            await this.sender.Send(new AddImageToBillCommand(this.currentUserId, id, image));
            return NoContent();
        }

        [HttpDelete("{id}/image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            await this.sender.Send(new DeleteImageFromBillCommand(this.currentUserId, id));
            return NoContent();
        }
    }
}