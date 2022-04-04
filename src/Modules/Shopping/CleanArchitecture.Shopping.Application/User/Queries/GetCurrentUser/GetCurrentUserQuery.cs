using System;
using CleanArchitecture.Shared.Application.Cqrs;
using CleanArchitecture.Shared.Core.Result;

namespace CleanArchitecture.Shopping.Application.User.Queries.GetCurrentUser
{
    public class GetCurrentUserQuery : IQuery<Result<UserDto>>
    {
        public GetCurrentUserQuery(Guid currentUserId)
        {
            CurrentUserId = currentUserId;
        }

        public Guid CurrentUserId { get; }
    }
}
