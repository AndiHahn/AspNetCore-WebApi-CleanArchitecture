using System;

namespace CleanArchitecture.Application.Services
{
    public interface ICurrentUserService
    {
        void SetCurrentUserId(Guid userId);
        Guid GetCurrentUserId();
    }
}
