using System;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.CrudServices;
using CleanArchitecture.Core.Models.Common;
using CleanArchitecture.Core.Models.Domain.Bill;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillService billService;

        public BillController(IBillService billService)
        {
            this.billService = billService ?? throw new ArgumentNullException(nameof(billService));
        }

        [HttpGet("query")]
        [ProducesResponseType(typeof(PagedResult<BillModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List([FromQuery] BillQueryParameter queryParameter)
        {
            return Ok(await billService.QueryAsync(queryParameter));
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<BillModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List([FromQuery] BillSearchParameter searchParameter)
        {
            return Ok(await billService.ListAsync(searchParameter));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BillModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await billService.GetByIdAsync(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BillModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddBill([FromBody] BillCreateModel createModel)
        {
            var createdBill = await billService.AddBillAsync(createModel);
            return Created($"{HttpContext.Request.Path}/{createdBill.Id}", createdBill);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BillModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBill(Guid id, [FromBody] BillUpdateModel updateModel)
        {
            return Ok(await billService.UpdateBillAsync(id, updateModel));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBill(Guid id)
        {
            await billService.DeleteBillAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/image")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetImage(Guid id)
        {
            var image = await billService.GetImageAsync(id);
            if (image == null) { return NoContent(); }
            return File(image.Content, image.ContentType);
        }

        [HttpPost("{id}/image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddImage(Guid id, [FromForm] IFormFile image)
        {
            await billService.AddImageToBillAsync(id, image);
            return NoContent();
        }

        [HttpDelete("{id}/image")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteImage(Guid id)
        {
            await billService.DeleteImageAsync(id);
            return NoContent();
        }

        [HttpGet("timerange")]
        [ProducesResponseType(typeof(TimeRangeModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAvailableTimeRange()
        {
            return Ok(await billService.GetAvailableTimeRangeAsync());
        }
    }
}