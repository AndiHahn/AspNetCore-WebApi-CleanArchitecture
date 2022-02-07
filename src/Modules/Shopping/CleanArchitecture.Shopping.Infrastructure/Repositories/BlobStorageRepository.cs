using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CleanArchitecture.Shared.Core.Models;
using CleanArchitecture.Shopping.Core.Interfaces;
using CleanArchitecture.Shopping.Infrastructure.AzureStorage;

namespace CleanArchitecture.Shopping.Infrastructure.Repositories
{
    internal class BlobStorageRepository : IBlobStorageRepository
    {
        private readonly IAzureStorageClientFactory azureStorageClientFactory;

        public BlobStorageRepository(IAzureStorageClientFactory azureStorageClientFactory)
        {
            this.azureStorageClientFactory = azureStorageClientFactory ?? throw new ArgumentNullException(nameof(azureStorageClientFactory));
        }

        public async Task<Blob> DownloadBlobAsync(string containerName, string blobUrl, CancellationToken cancellationToken = default)
        {
            var blobClient = azureStorageClientFactory.GetBlobClient(containerName, blobUrl);
            if (!await blobClient.ExistsAsync(cancellationToken))
            {
                return null;
            }

            var downloadInfo = (await blobClient.DownloadAsync(cancellationToken)).Value;

            return new Blob(downloadInfo.Content, downloadInfo.ContentType, downloadInfo.Details.LastModified.DateTime);
        }

        public async Task UploadBlobAsync(string containerName, string blobUrl, Blob blob, CancellationToken cancellationToken = default)
        {
            await using var stream = blob.Content;
            BlobClient blobClient = azureStorageClientFactory.GetBlobClient(containerName, blobUrl);
            await blobClient.UploadAsync(
                stream,
                new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders()
                    {
                        ContentType = blob.ContentType
                    }
                },
                cancellationToken);
        }

        public async Task RemoveBlobAsync(string containerName, string blobUrl, CancellationToken cancellationToken = default)
        {
            BlobClient blobClient = azureStorageClientFactory.GetBlobClient(containerName, blobUrl);
            if (!await blobClient.ExistsAsync(cancellationToken))
            {
                throw new ArgumentException($"Blob {blobUrl} is not available.");
            }

            await blobClient.DeleteAsync(cancellationToken: cancellationToken);
        }

        public async Task<bool> RemoveBlobIfExistsAsync(string containerName, string blobUrl, CancellationToken cancellationToken = default)
        {
            BlobClient blobClient = azureStorageClientFactory.GetBlobClient(containerName, blobUrl);
            if (await blobClient.ExistsAsync(cancellationToken))
            {
                await blobClient.DeleteAsync(cancellationToken: cancellationToken);
                return true;
            }

            return false;
        }

        public Task<bool> BlobExistsAsync(string containerName, string blobUrl, CancellationToken cancellationToken = default)
        {
            BlobClient blobClient = azureStorageClientFactory.GetBlobClient(containerName, blobUrl);
            return blobClient.ExistsAsync(cancellationToken).ContinueWith(r => r.Result.Value);
        }
    }
}
