using System;
using CleanArchitecture.Shared.Application;
using CleanArchitecture.Shared.Core;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Web.Blazor.Modules.Core.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public Guid GetCurrentUserId()
        {
            return httpContextAccessor.HttpContext.User.GetUserId();
        }
    }
}
