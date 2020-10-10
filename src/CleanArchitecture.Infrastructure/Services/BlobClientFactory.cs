using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using CleanArchitecture.Core;
using CleanArchitecture.Core.Configurations;
using CleanArchitecture.Core.Interfaces.BlobStorage;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Infrastructure.Services
{
    public class BlobClientFactory : IBlobClientFactory
    {
        private readonly BlobStorageConfiguration storageConfiguration;

        public BlobClientFactory(IOptions<BlobStorageConfiguration> storageConfiguration)
        {
            this.storageConfiguration = storageConfiguration?.Value ?? throw new ArgumentNullException(nameof(storageConfiguration));
        }

        public async Task<BlobClient> CreateClientAsync(string fileUrl)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageConfiguration.ConnectionString);
            string containerName = Constants.BlobStorage.CONTAINER_NAME;

            BlobContainerClient client = blobServiceClient.GetBlobContainerClient(containerName.ToLower());
            if (!await client.ExistsAsync())
            {
                await client.CreateAsync();
            }

            BlobClient blobClient = client.GetBlobClient(fileUrl);
            return blobClient;
        }
    }
}
