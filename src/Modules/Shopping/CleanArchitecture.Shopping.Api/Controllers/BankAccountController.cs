using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Application;
using CleanArchitecture.Shared.Core.Filter;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Api.Dtos;
using CleanArchitecture.Shopping.Application.BankAccount;
using CleanArchitecture.Shopping.Application.BankAccount.Commands.CreateBankAccount;
using CleanArchitecture.Shopping.Application.BankAccount.Commands.ShareAccountWithUser;
using CleanArchitecture.Shopping.Application.BankAccount.Queries.GetBankAccountById;
using CleanArchitecture.Shopping.Application.BankAccount.Queries.GetBankAccounts;
using CleanArchitecture.Shopping.Application.BankAccount.Queries.GetSharedAccounts;
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
    public class BankAccountController : ControllerBase
    {
        private readonly ISender sender;
        private readonly Guid currentUserId;

        public BankAccountController(
            ISender sender,
            IHttpContextAccessor httpContextAccessor)
        {
            currentUserId = httpContextAccessor.HttpContext?.User.GetUserId() ?? Guid.Empty;
            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BankAccountDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public Task<Result<IEnumerable<BankAccountDto>>> GetAll(CancellationToken cancellationToken)
            => this.sender.Send(new GetBankAccountsQuery(this.currentUserId), cancellationToken);

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BankAccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<Result<BankAccountDto>> GetById(Guid id, CancellationToken cancellationToken)
            => this.sender.Send(new GetBankAccountByIdQuery(this.currentUserId, id), cancellationToken);

        [HttpPost]
        [ProducesResponseType(typeof(BankAccountDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateAccount([FromBody] BankAccountCreateDto dto, CancellationToken cancellationToken)
        {
            var account = await this.sender.Send(new CreateBankAccountCommand(this.currentUserId, dto.Name), cancellationToken);

            if (account.Status != ResultStatus.Success) return this.ToActionResult(account);

            return Created($"{HttpContext.Request.Path}/{account.Value.Id}", account.Value);
        }

        [HttpPut("account/{accountId}/share/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<Result> ShareAccountWithUser(Guid accountId, Guid userId, CancellationToken cancellationToken)
            => this.sender.Send(new ShareAccountWithUserCommand(accountId, userId, currentUserId), cancellationToken);

        [HttpGet("account/shared")]
        [ProducesResponseType(typeof(IEnumerable<BankAccountDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public Task<Result<IEnumerable<BankAccountDto>>> GetSharedAccounts(CancellationToken cancellationToken)
            => sender.Send(new GetSharedAccountsQuery(currentUserId), cancellationToken);
    }
}
