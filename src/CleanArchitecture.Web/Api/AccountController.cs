using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Services.Account;
using CleanArchitecture.Services.Account.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Web.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly ILogger<AccountController> logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AccountModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation("Get all available accounts...");
            return Ok(await accountService.GetAllAsync());
        }
    }
}
