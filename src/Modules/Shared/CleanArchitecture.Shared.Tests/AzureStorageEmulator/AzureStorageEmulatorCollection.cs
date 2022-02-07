using Xunit;

namespace CleanArchitecture.Shared.Tests.AzureStorageEmulator
{
    [CollectionDefinition("AzureStorageIntegrationTests")]
    public class AzureStorageEmulatorCollection : ICollectionFixture<AzureStorageEmulatorFixture>
    {
    }
}
