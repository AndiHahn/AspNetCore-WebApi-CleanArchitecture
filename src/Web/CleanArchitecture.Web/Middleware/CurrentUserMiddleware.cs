using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CleanArchitecture.Application.Services;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Web.Api.Middleware
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate next;

        public CurrentUserMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(
            HttpContext httpContext,
            ICurrentUserService currentUserService)
        {
            var userIdClaim = httpContext.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                currentUserService.SetCurrentUserId(Guid.Parse(userIdClaim.Value));
            }

            await next.Invoke(httpContext);
        }
    }
}
