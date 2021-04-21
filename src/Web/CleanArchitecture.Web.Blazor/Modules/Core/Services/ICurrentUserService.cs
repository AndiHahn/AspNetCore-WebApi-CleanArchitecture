using System;

namespace CleanArchitecture.Web.Blazor.Modules.Core.Services
{
    public interface ICurrentUserService
    {
        Guid GetCurrentUserId();
    }
}
