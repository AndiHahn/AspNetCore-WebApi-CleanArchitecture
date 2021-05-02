using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Configurations;
using CleanArchitecture.Application.CrudServices.Interfaces;
using CleanArchitecture.Application.CrudServices.Models.User;
using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Web.Api.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IUserService userService;
        private readonly AuthenticationConfiguration configuration;
        private readonly ILogger<UserController> logger;

        public UserController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IOptions<AuthenticationConfiguration> configuration,
            IUserService userService,
            ILogger<UserController> logger)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
            this.configuration = configuration.Value ?? throw new ArgumentNullException(nameof(configuration));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Authenticate([FromBody] SignInModel model)
        {
            logger.LogInformation($"User authentication: {model.Username}");

            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                throw new NotFoundException($"User {model.Username} not found.");
            }

            if (!await userManager.CheckPasswordAsync(user, model.Password))
            {
                throw new UnauthorizedException("Invalid login credentials.");
            }

            DateTime expires = DateTime.UtcNow.AddMinutes(60);
            string token = await GenerateTokenAsync(user, expires);

            var authUserModel = new AuthenticationResponse
            {
                Id = new Guid(user.Id),
                Username = user.UserName,
                EmailAddress = user.Email,
                Token = token
            };

            return Ok(authUserModel);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<UserModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List([FromQuery] UserQueryParameter queryParameter)
        {
            var users = await userService.ListAsync(queryParameter);
            return Ok(users);
        }

        private async Task<string> GenerateTokenAsync(IdentityUser user, DateTime expires)
        {
            var principal = await signInManager.CreateUserPrincipalAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>();

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(principal.Identity, claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = expires,
                Issuer = "https://localhost/cleanarchitecture",
                Audience = "cleanarchitecture-api",
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }
    }
}