using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CleanArchitecture.Core.Interfaces.BlobStorage;
using CleanArchitecture.Core.Interfaces.Image;
using Microsoft.AspNetCore.Http;

namespace CleanArchitecture.Core.Services.Image
{
    public class ImageStorageService : IImageStorageService
    {
        private readonly IBlobClientFactory blobClientFactory;

        public ImageStorageService(IBlobClientFactory blobClientFactory)
        {
            this.blobClientFactory = blobClientFactory ?? throw new ArgumentNullException(nameof(blobClientFactory));
        }

        public async Task<BlobDownloadInfo> GetImageDownloadInfoAsync(string fileUrl)
        {
            BlobClient blobClient = await blobClientFactory.CreateClientAsync(fileUrl);
            if (!await blobClient.ExistsAsync())
            {
                return null;
            }
            return await blobClient.DownloadAsync();
        }

        public async Task UploadImageAsync(string fileUrl, IFormFile image)
        {
            using (var stream = image.OpenReadStream())
            {
                BlobClient blobClient = await blobClientFactory.CreateClientAsync(fileUrl);
                await blobClient.UploadAsync(stream, httpHeaders: new BlobHttpHeaders()
                {
                    ContentType = image.ContentType
                });
            }
        }

        public async Task RemoveImageAsync(string fileUrl)
        {
            BlobClient blobClient = await blobClientFactory.CreateClientAsync(fileUrl);
            if (!await blobClient.ExistsAsync())
            {
                throw new ArgumentException($"Image {fileUrl} is not available.");
            }

            await blobClient.DeleteAsync();
        }

        public async Task RemoveImageIfExistsAsync(string fileUrl)
        {
            BlobClient blobClient = await blobClientFactory.CreateClientAsync(fileUrl);
            if (await blobClient.ExistsAsync())
            {
                await blobClient.DeleteAsync();
            }
        }
    }
}
