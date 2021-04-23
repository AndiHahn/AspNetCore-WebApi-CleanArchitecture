using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.BlobEntities;
using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Domain.Interfaces;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.Infrastructure.Services.AzureStorage;
using Microsoft.Extensions.Options;
using Xunit;

namespace CleanArchitecture.IntegrationTests.Infrastructure
{
    [Collection("AzureStorageIntegrationTests")]
    public class BlobStorageRepositoryTests
    {
        private readonly string containerName = "containername";
        private readonly string blobUrl = "blob-" + Guid.NewGuid();

        [Fact]
        public async Task UploadBlob_ShouldUploadBlob_Successfully()
        {
            string uploadText = "TextBlobContent";
            var repository = await SetupRepositoryAsync();
            byte[] byteArray = Encoding.ASCII.GetBytes(uploadText);
            MemoryStream stream = new MemoryStream(byteArray);
            var blobEntity = new BlobEntity
            {
                Content = stream,
                ContentType = "text/plain"
            };

            await repository.UploadBlobAsync(containerName, blobUrl, blobEntity);
            var downloadedBlob = await repository.DownloadBlobAsync(containerName, blobUrl);

            Assert.NotNull(downloadedBlob);
            Assert.Equal(blobEntity.ContentType, downloadedBlob.ContentType);
            StreamReader reader = new StreamReader(downloadedBlob.Content);
            string downloadedText = reader.ReadToEnd();
            Assert.Equal(uploadText, downloadedText);
        }

        [Fact]
        public async Task DownloadBlob_ShouldThrowException_IfBlobNotAvailable()
        {
            var repository = await SetupRepositoryAsync();
            await Assert.ThrowsAsync<NotFoundException>(() =>
                        repository.DownloadBlobAsync(containerName, blobUrl));
        }

        private async Task<IBlobStorageRepository> SetupRepositoryAsync()
        {
            var storageConfiguration = new AzureStorageConfiguration
            {
                ConnectionString = "UseDevelopmentStorage=true"
            };
            var storageOptions = Options.Create(storageConfiguration);
            var clientFactory = new AzureStorageClientFactory(storageOptions);
            await clientFactory.CreateBlobContainerIfNotExistsAsync(containerName);
            return new BlobStorageRepository(clientFactory);
        }
    }
}
