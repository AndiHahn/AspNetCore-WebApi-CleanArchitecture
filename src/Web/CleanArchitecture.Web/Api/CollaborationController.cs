﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Application.UseCases.Collaboration.Commands;
using CleanArchitecture.Application.UseCases.Collaboration.Queries;
using CleanArchitecture.Core.Models.Domain.BankAccount;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Api.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CollaborationController : ControllerBase
    {
        private readonly IMediator mediator;

        public CollaborationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPut("account/{accountId}/share/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ShareAccountWithUser(Guid accountId, Guid userId)
        {
            await mediator.Send(new ShareAccountWithUserCommand(accountId, userId));
            return NoContent();
        }

        [HttpGet("account/shared")]
        [ProducesResponseType(typeof(IEnumerable<BankAccountModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetSharedAccounts()
        {
            var result = await mediator.Send(new GetSharedAccountsQuery());
            return Ok(result);
        }
    }
}