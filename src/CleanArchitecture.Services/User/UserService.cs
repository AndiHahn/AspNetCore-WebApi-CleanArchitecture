using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Configurations;
using CleanArchitecture.Core.Helper;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.QueryParameter;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Services.User.Extensions;
using CleanArchitecture.Services.User.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Services.User
{
    public class UserService : IUserService
    {
        private readonly IBudgetContext context;
        private readonly AuthenticationConfiguration configuration;

        public UserService(IBudgetContext context, IOptions<AuthenticationConfiguration> configuration)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.configuration = configuration.Value ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<AuthenticatedUserModel> AuthenticateAsync(SignInModel model)
        {
            var user = (await context.User
                                    .FirstOrDefaultAsync(u => u.UserName == model.UserName))
                                    .AssertEntityFound();

            PasswordHelper.IsEqual(model.Password, user.Password, user.Salt)
                          .AssertIsTrue("Password not correct.");

            // authentication successful so generate jwt token
            var token = GenerateJwtToken(user);

            return new AuthenticatedUserModel(user, token);
        }

        public async Task<IEnumerable<UserModel>> ListAsync(UserQueryParameter queryParameter)
        {
            return (await context.User.ApplyPaging(queryParameter).ToListAsync()).Select(u => u.ToModel());
        }

        private string GenerateJwtToken(UserEntity user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}