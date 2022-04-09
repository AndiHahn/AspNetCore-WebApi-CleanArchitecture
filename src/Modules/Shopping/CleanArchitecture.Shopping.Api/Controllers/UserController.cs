using System;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Core.Filter;
using CleanArchitecture.Shared.Core.Result;
using CleanArchitecture.Shopping.Api.Dtos;
using CleanArchitecture.Shopping.Application.User;
using CleanArchitecture.Shopping.Application.User.Queries.AuthenticateUser;
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
    public class UserController : ControllerBase
    {
        private readonly ISender sender;

        public UserController(ISender sender)
        {
            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(typeof(AuthenticationResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public Task<Result<AuthenticationResponseDto>> Authenticate(
            [FromBody] SignInDto dto,
            CancellationToken cancellationToken)
            => this.sender.Send(new AuthenticateUserQuery(dto.Username, dto.Password), cancellationToken);
    }
}