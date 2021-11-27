using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Application.Models;
using CleanArchitecture.Core.Interfaces;
using MediatR;

namespace CleanArchitecture.Application.User
{
    public class GetCurrentUserQuery : IRequest<Result<UserDto>>
    {
        public GetCurrentUserQuery(Guid currentUserId)
        {
            CurrentUserId = currentUserId;
        }

        public Guid CurrentUserId { get; }
    }

    internal class GetUserQueryHandler : IRequestHandler<GetCurrentUserQuery, Result<UserDto>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetUserQueryHandler(
            IUserRepository userRepository,
            IMapper mapper)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<Result<UserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            return this.userRepository
                .GetByIdAsync(request.CurrentUserId, cancellationToken)
                .ContinueWith(u => Result<UserDto>.Success(this.mapper.Map<UserDto>(u.Result)));
        }
    }
}
