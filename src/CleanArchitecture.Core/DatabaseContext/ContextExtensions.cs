using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Infrastructure;
using CleanArchitecture.Domain.Base;

namespace CleanArchitecture.Core.DatabaseContext
{
    public static class ContextExtensions
    {
        public static void UpdateVersion(this IBudgetContext context, object entity, byte[] suspectedVersion)
        {
            var versionProperty = context.Entry(entity).Property(nameof(VersionableEntity.Version));
            versionProperty.OriginalValue = suspectedVersion;
        }
    }
}