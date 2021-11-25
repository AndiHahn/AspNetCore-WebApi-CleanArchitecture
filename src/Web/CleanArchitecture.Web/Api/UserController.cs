using System;
using System.Threading.Tasks;
using CleanArchitecture.Application.User;
using CleanArchitecture.Web.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Api
{
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
        public async Task<IActionResult> Authenticate([FromBody] SignInDto dto)
            => Ok(await this.sender.Send(new AuthenticateUserCommand(dto.Username, dto.Password)));
    }
}