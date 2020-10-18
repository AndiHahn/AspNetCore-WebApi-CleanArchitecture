using Xunit;

namespace CleanArchitecture.Tests.Shared.AzureStorageEmulator
{
    [CollectionDefinition("AzureStorageIntegrationTests")]
    public class AzureStorageEmulatorCollection : ICollectionFixture<AzureStorageEmulatorFixture>
    {
    }
}
