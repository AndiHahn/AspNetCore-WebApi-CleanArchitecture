using Xunit;

namespace CleanArchitecture.IntegrationTests.Setup
{
    [CollectionDefinition("AzureStorageIntegrationTests")]
    public class AzureStorageEmulatorCollection : ICollectionFixture<AzureStorageEmulatorFixture>
    {
    }
}
