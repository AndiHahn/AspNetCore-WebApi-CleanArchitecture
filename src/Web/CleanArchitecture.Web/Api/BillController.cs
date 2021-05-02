using System;
using System.Threading.Tasks;
using CleanArchitecture.Application.CrudServices;
using CleanArchitecture.Application.CrudServices.Interfaces;
using CleanArchitecture.Application.CrudServices.Models.Bill;
using CleanArchitecture.Application.Extensions;
using CleanArchitecture.Common.Models.Resource.Bill;
using CleanArchitecture.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Api.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillService billService;
        private readonly Guid currentUserId;

        public BillController(
            IBillService billService,
            IHttpContextAccessor httpContextAccessor)
        {
            currentUserId = httpContextAccessor.HttpContext.User.GetUserId();
            this.billService = billService ?? throw new ArgumentNullException(nameof(billService));
        }

        [HttpGet("query")]
        [ProducesResponseType(typeof(PagedResult<BillModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List([FromQuery] BillQueryParameter queryParameter)
        {
            return Ok(await billService.QueryAsync(queryParameter, currentUserId));
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<BillModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List([FromQuery] BillSearchParameter searchParameter)
        {
            return Ok(await billService.ListAsync(searchParameter, currentUserId));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BillModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await billService.GetByIdAsync(id, currentUserId));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BillModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateBill([FromBody] BillCreateModel createModel)
        {
            var createdBill = await billService.CreateBillAsync(createModel, currentUserId);
            return Created($"{HttpContext.Request.Path}/{createdBill.Id}", createdBill);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BillModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBill(Guid id, [FromBody] BillUpdateModel updateModel)
        {
            return Ok(await billService.UpdateBillAsync(id, updateModel, currentUserId));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBill(Guid id)
        {
            await billService.DeleteBillAsync(id, currentUserId);
            return NoContent();
        }

        [HttpGet("{id}/image")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImage(Guid id)
        {
            var image = await billService.GetImageAsync(id, currentUserId);
            return File(image.Content, image.ContentType);
        }

        [HttpPost("{id}/image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddImage(Guid id, [FromForm] IFormFile image)
        {
            await billService.AddImageToBillAsync(id, image, currentUserId);
            return NoContent();
        }

        [HttpDelete("{id}/image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            await billService.DeleteImageAsync(id, currentUserId);
            return NoContent();
        }
    }
}