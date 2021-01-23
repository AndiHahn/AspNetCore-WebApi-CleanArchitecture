using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces.CrudServices;
using CleanArchitecture.Core.Models.Domain.BankAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Web.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService bankAccountService;
        private readonly ILogger<BankAccountController> logger;

        public BankAccountController(IBankAccountService bankAccountService, ILogger<BankAccountController> logger)
        {
            this.bankAccountService = bankAccountService ?? throw new ArgumentNullException(nameof(bankAccountService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BankAccountModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("Get all available accounts...");
            return Ok(await bankAccountService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BankAccountModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await bankAccountService.GetByIdAsync(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BankAccountModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateAccount([FromBody] BankAccountCreateModel createModel)
        {
            var createdBill = await bankAccountService.CreateAccountAsync(createModel);
            return Created($"{HttpContext.Request.Path}/{createdBill.Id}", createdBill);
        }
    }
}
