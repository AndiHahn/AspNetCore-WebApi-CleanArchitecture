using System.Security.Claims;

namespace CleanArchitecture.Shared.Core
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                throw new InvalidOperationException("Claims principal does not contain nameidentifier");
            }

            if (!Guid.TryParse(claim.Value, out Guid userId))
            {
                throw new ArgumentException("Could not parse userId to Guid");
            }

            return userId;
        }

        public static bool TryGetUserId(this ClaimsPrincipal claimsPrincipal, out Guid userId)
        {
            userId = default;

            var claim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return false;
            }

            if (Guid.TryParse(claim.Value, out userId))
            {
                return true;
            }

            return false;
        }
    }
}
