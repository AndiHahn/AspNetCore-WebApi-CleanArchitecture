using CleanArchitecture.Core.Models;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Repositories
{
    internal class BillImageRepository : IBillImageRepository
    {
        private readonly IBlobStorageRepository blobStorageRepository;

        private readonly string blobContainerName = "bills";
        private readonly Func<Guid, string> imagePathFunc = (billId) => $"images/{billId}";

        public BillImageRepository(
            IBlobStorageRepository blobStorageRepository)
        {
            this.blobStorageRepository = blobStorageRepository ?? throw new ArgumentNullException(nameof(blobStorageRepository));
        }

        public async Task UploadImageAsync(Guid billId, IFormFile image, CancellationToken cancellationToken = default)
        {
            await using var blob = new Blob(image.ContentType);
            await image.CopyToAsync(blob.Content, cancellationToken);
            blob.Reset();

            await this.blobStorageRepository.UploadBlobAsync(blobContainerName, this.imagePathFunc(billId), blob);
        }

        public Task DeleteImageAsync(Guid billId, CancellationToken cancellationToken = default)
        {
            return this.blobStorageRepository.RemoveBlobAsync(blobContainerName, this.imagePathFunc(billId));
        }

        public Task<Blob> DownloadImageAsync(Guid billId, CancellationToken cancellationToken = default)
        {
            return this.blobStorageRepository.DownloadBlobAsync(blobContainerName, this.imagePathFunc(billId));
        }

        public Task<bool> ImageExistsAsync(Guid billId, CancellationToken cancellationToken = default)
        {
            return this.blobStorageRepository.BlobExistsAsync(blobContainerName, this.imagePathFunc(billId), cancellationToken);
        }
    }
}
