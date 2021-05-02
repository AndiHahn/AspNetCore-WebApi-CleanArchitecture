using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.CrudServices.Interfaces;
using CleanArchitecture.Application.CrudServices.Models.User;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Domain.Interfaces.Repositories;
using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Application.CrudServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PagedResult<UserModel>> ListAsync(UserQueryParameter queryParameter)
        {
            var pagedResult = await userRepository.ListAsync(queryParameter);

            return new PagedResult<UserModel>
            {
                Result = pagedResult.Result.Select(mapper.Map<UserModel>),
                TotalCount = pagedResult.TotalCount
            };
        }
    }
}