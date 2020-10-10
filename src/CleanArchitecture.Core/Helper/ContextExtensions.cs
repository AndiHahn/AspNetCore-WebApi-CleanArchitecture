using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Domain.Base;

namespace CleanArchitecture.Core.Helper
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