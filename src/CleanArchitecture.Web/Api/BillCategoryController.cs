using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.Services.BillCategory;
using CleanArchitecture.Core.Interfaces.Services.BillCategory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Web.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BillCategoryController : ControllerBase
    {
        private readonly IBillCategoryService billCategoryService;
        private readonly ILogger<BillCategoryController> logger;

        public BillCategoryController(IBillCategoryService billCategoryService, ILogger<BillCategoryController> logger)
        {
            this.billCategoryService = billCategoryService ?? throw new ArgumentNullException(nameof(billCategoryService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BillCategoryModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("Get all available bill categories...");
            return Ok(await billCategoryService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BillCategoryModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await billCategoryService.GetByIdAsync(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BillCategoryModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] BillCategoryCreateModel createModel)
        {
            var createdCategory = await billCategoryService.CreateAsync(createModel);
            return Created($"{HttpContext.Request.Path}/{createdCategory.Id}", createdCategory);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] BillCategoryUpdateModel updateModel)
        {
            await billCategoryService.UpdateAsync(id, updateModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await billCategoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
