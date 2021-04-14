using System;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.Services;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Web.Api.Middleware
{
    public class CurrentUserMiddleware
    {
        public const string UserIdClaimIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
        private readonly RequestDelegate next;

        public CurrentUserMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(
            HttpContext httpContext,
            ICurrentUserService currentUserService)
        {
            var userIdClaim = httpContext.User?.Claims?.FirstOrDefault(c => c.Type == UserIdClaimIdentifier);
            if (userIdClaim != null)
            {
                currentUserService.SetCurrentUserId(Guid.Parse(userIdClaim.Value));
            }

            await next.Invoke(httpContext);
        }
    }
}
