using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Infrastructure.Services.AzureStorage
{
    public class AzureStorageClientFactory : IAzureStorageClientFactory
    {
        private readonly AzureStorageConfiguration storageConfiguration;
        private CloudTableClient tableClient;
        private BlobServiceClient blobServiceClient;

        public AzureStorageClientFactory(IOptions<AzureStorageConfiguration> storageConfiguration)
        {
            this.storageConfiguration = storageConfiguration?.Value ?? throw new ArgumentNullException(nameof(storageConfiguration));
        }

        public async Task<CloudTableClient> GetOrCreateTableClientAsync(string tableName)
        {
            CloudTableClient client = GetTableClient();
            var tableReference = client.GetTableReference(tableName);
            if (!await tableReference.ExistsAsync())
            {
                await tableReference.CreateAsync();
            }

            return client;
        }

        public async Task CreateTableIfNotExistsAsync(string tableName)
        {
            CloudTableClient client = GetTableClient();
            var tableReference = client.GetTableReference(tableName);
            if (!await tableReference.ExistsAsync())
            {
                await tableReference.CreateAsync();
            }
        }

        public CloudTableClient GetTableClient()
        {
            tableClient ??= CloudStorageAccount.Parse(storageConfiguration.ConnectionString).CreateCloudTableClient();
            return tableClient;
        }

        public async Task<BlobClient> GetOrCreateBlobClientAsync(string containerName, string blobName)
        {
            var client = GetBlobContainerClient(containerName);
            if (!await client.ExistsAsync())
            {
                await client.CreateAsync();
            }

            return client.GetBlobClient(blobName);
        }

        public async Task CreateBlobContainerIfNotExistsAsync(string containerName)
        {
            var client = GetBlobContainerClient(containerName);
            if (!await client.ExistsAsync())
            {
                await client.CreateAsync();
            }
        }

        public BlobClient GetBlobClient(string containerName, string blobName)
        {
            var client = GetBlobContainerClient(containerName);
            return client.GetBlobClient(blobName);
        }

        private BlobContainerClient GetBlobContainerClient(string containerName)
        {
            blobServiceClient ??= new BlobServiceClient(storageConfiguration.ConnectionString);
            return blobServiceClient.GetBlobContainerClient(containerName.ToLower());
        }
    }
}
