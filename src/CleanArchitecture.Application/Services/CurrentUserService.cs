using System;

namespace CleanArchitecture.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private Guid userId;

        public Guid GetCurrentUserId()
        {
            return userId;
        }

        public void SetCurrentUserId(Guid userId)
        {
            this.userId = userId;
        }
    }
}
