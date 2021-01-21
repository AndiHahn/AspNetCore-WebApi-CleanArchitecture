using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CleanArchitecture.Domain.BlobEntities;
using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Infrastructure.Services.AzureStorage;

namespace CleanArchitecture.Infrastructure.Repositories
{
    public class BlobStorageRepository : IBlobStorageRepository
    {
        private readonly IAzureStorageClientFactory azureStorageClientFactory;

        public BlobStorageRepository(IAzureStorageClientFactory azureStorageClientFactory)
        {
            this.azureStorageClientFactory = azureStorageClientFactory ?? throw new ArgumentNullException(nameof(azureStorageClientFactory));
        }

        public async Task<BlobDownloadEntity> DownloadBlobAsync(string containerName, string blobUrl)
        {
            var blobClient = azureStorageClientFactory.GetBlobClient(containerName, blobUrl);
            if (!await blobClient.ExistsAsync())
            {
                throw new NotFoundException($"Blob with url {blobUrl} not available.");
            }

            var blobDownloadInfo = (await blobClient.DownloadAsync()).Value;

            return new BlobDownloadEntity
            {
                Content = blobDownloadInfo.Content,
                ContentType = blobDownloadInfo.ContentType,
                LastModified = blobDownloadInfo.Details.LastModified.DateTime
            };
        }

        public async Task UploadBlobAsync(string containerName, string blobUrl, BlobUploadModel blob)
        {
            await using var stream = blob.Content;
            BlobClient blobClient = azureStorageClientFactory.GetBlobClient(containerName, blobUrl);
            await blobClient.UploadAsync(stream, new BlobHttpHeaders
            {
                ContentType = blob.ContentType
            });
        }

        public async Task RemoveBlobAsync(string containerName, string blobUrl)
        {
            BlobClient blobClient = azureStorageClientFactory.GetBlobClient(containerName, blobUrl);
            if (!await blobClient.ExistsAsync())
            {
                throw new ArgumentException($"Blob {blobUrl} is not available.");
            }

            await blobClient.DeleteAsync();
        }

        public async Task<bool> RemoveBlobIfExistsAsync(string containerName, string blobUrl)
        {
            BlobClient blobClient = azureStorageClientFactory.GetBlobClient(containerName, blobUrl);
            if (await blobClient.ExistsAsync())
            {
                await blobClient.DeleteAsync();
                return true;
            }

            return false;
        }
    }
}
