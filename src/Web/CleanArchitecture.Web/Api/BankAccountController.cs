using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Application.BankAccount;
using CleanArchitecture.Application.User;
using CleanArchitecture.Web.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Api.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly ISender sender;
        private readonly Guid currentUserId;

        public BankAccountController(
            ISender sender,
            IHttpContextAccessor httpContextAccessor)
        {
            currentUserId = httpContextAccessor.HttpContext.User.GetUserId();
            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BankAccountDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAll()
            => Ok(await this.sender.Send(new GetBankAccountsQuery(this.currentUserId)));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BankAccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
            => Ok(await this.sender.Send(new GetBankAccountByIdQuery(this.currentUserId, id)));

        [HttpPost]
        [ProducesResponseType(typeof(BankAccountDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateAccount([FromBody] BankAccountCreateDto dto)
        {
            var account = await this.sender.Send(new CreateBankAccountCommand(this.currentUserId, dto.Name));
            return Created($"{HttpContext.Request.Path}/{account.Id}", account);
        }

        [HttpPut("account/{accountId}/share/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ShareAccountWithUser(Guid accountId, Guid userId)
        {
            await this.sender.Send(new ShareAccountWithUserCommand(accountId, userId, currentUserId));
            return NoContent();
        }

        [HttpGet("account/shared")]
        [ProducesResponseType(typeof(IEnumerable<BankAccountDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSharedAccounts()
            => Ok(await sender.Send(new GetSharedAccountsQuery(currentUserId)));
    }
}
