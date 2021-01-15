using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Core.Configurations;
using CleanArchitecture.Core.GenericQuery;
using CleanArchitecture.Core.Interfaces.Infrastructure;
using CleanArchitecture.Core.Interfaces.Models;
using CleanArchitecture.Core.Interfaces.Services.User;
using CleanArchitecture.Core.Interfaces.Services.User.Models;
using CleanArchitecture.Core.Validations;
using CleanArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Core.CrudServices
{
    public class UserService : IUserService
    {
        private readonly IBudgetContext context;
        private readonly IMapper mapper;
        private readonly AuthenticationConfiguration configuration;

        public UserService(
                IBudgetContext context,
                IMapper mapper,
                IOptions<AuthenticationConfiguration> configuration)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.configuration = configuration.Value ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<AuthenticatedUserModel> AuthenticateAsync(SignInModel model)
        {
            var user = (await context.User
                                    .FirstOrDefaultAsync(u => u.UserName == model.UserName))
                                    .AssertEntityFound();

            var validPassword = new HashedPassword();
            validPassword.WithHashAndSalt(user.Password, user.Salt);

            var givenPassword = new HashedPassword();
            givenPassword.WithPlainPasswordAndSalt(model.Password, validPassword.Salt);

            validPassword.Equals(givenPassword).AssertIsTrue("Password is wrong.");

            // authentication successful so generate jwt token
            var token = GenerateJwtToken(user);

            return new AuthenticatedUserModel(user, token);
        }

        public async Task<IEnumerable<UserModel>> ListAsync(UserQueryParameter queryParameter)
        {
            return (await context.User.ApplyPaging(queryParameter).ToListAsync())
                            .Select(u => mapper.Map<UserModel>(u));
        }

        private string GenerateJwtToken(UserEntity user)
        {
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