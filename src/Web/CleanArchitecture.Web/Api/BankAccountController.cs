using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Application.CrudServices;
using CleanArchitecture.Application.CrudServices.Interfaces;
using CleanArchitecture.Application.CrudServices.Models.BankAccount;
using CleanArchitecture.Application.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Web.Api.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService bankAccountService;
        private readonly ILogger<BankAccountController> logger;
        private readonly Guid currentUserId;

        public BankAccountController(
            IBankAccountService bankAccountService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<BankAccountController> logger)
        {
            currentUserId = httpContextAccessor.HttpContext.User.GetUserId();
            this.bankAccountService = bankAccountService ?? throw new ArgumentNullException(nameof(bankAccountService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BankAccountModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("Get all available accounts...");
            return Ok(await bankAccountService.GetAllAsync(currentUserId));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BankAccountModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await bankAccountService.GetByIdAsync(id, currentUserId));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BankAccountModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateAccount([FromBody] BankAccountCreateModel createModel)
        {
            var createdBill = await bankAccountService.CreateAccountAsync(createModel, currentUserId);
            return Created($"{HttpContext.Request.Path}/{createdBill.Id}", createdBill);
        }
    }
}
