using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Application.BankAccount;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Application.User;
using CleanArchitecture.Web.Api.Filter;
using CleanArchitecture.Web.Api.Dtos;
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
        public Task<Result<IEnumerable<BankAccountDto>>> GetAll()
            => this.sender.Send(new GetBankAccountsQuery(this.currentUserId));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BankAccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<Result<BankAccountDto>> GetById(Guid id)
            => this.sender.Send(new GetBankAccountByIdQuery(this.currentUserId, id));

        [HttpPost]
        [ProducesResponseType(typeof(BankAccountDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateAccount([FromBody] BankAccountCreateDto dto)
        {
            var account = await this.sender.Send(new CreateBankAccountCommand(this.currentUserId, dto.Name));

            if (account.Status != ResultStatus.Success) return this.ToActionResult(account);

            return Created($"{HttpContext.Request.Path}/{account.Value.Id}", account.Value);
        }

        [HttpPut("account/{accountId}/share/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<Result> ShareAccountWithUser(Guid accountId, Guid userId)
            => this.sender.Send(new ShareAccountWithUserCommand(accountId, userId, currentUserId));

        [HttpGet("account/shared")]
        [ProducesResponseType(typeof(IEnumerable<BankAccountDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public Task<Result<IEnumerable<BankAccountDto>>> GetSharedAccounts()
            => sender.Send(new GetSharedAccountsQuery(currentUserId));
    }
}
