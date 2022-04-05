using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Shared.Core.Models;
using CleanArchitecture.Shopping.Core.Interfaces;
using CleanArchitecture.Shopping.Infrastructure.AzureStorage;
using CleanArchitecture.Shopping.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace CleanArchitecture.Shopping.IntegrationTests.Infrastructure
{
    [Collection("AzureStorageIntegrationTests")]
    public class BlobStorageRepositoryTests
    {
        private readonly string containerName = "containername";
        private readonly string blobUrl = "blob-" + Guid.NewGuid();

        [Fact]
        public async Task UploadBlob_ShouldUploadBlob_Successfully()
        {
            // Arrange
            string uploadText = "TextBlobContent";
            var repository = await SetupRepositoryAsync();
            byte[] byteArray = Encoding.ASCII.GetBytes(uploadText);
            await using MemoryStream stream = new MemoryStream(byteArray);
            var blobEntity = new Blob(stream, "text/plain");

            // Act
            await repository.UploadBlobAsync(containerName, blobUrl, blobEntity);
            
            // Assert
            var downloadedBlob = await repository.DownloadBlobAsync(containerName, blobUrl);
            downloadedBlob.Should().NotBeNull();
            downloadedBlob.ContentType.Should().Be(blobEntity.ContentType);

            using StreamReader reader = new StreamReader(downloadedBlob.Content);
            string downloadedText = await reader.ReadToEndAsync();
            downloadedText.Should().Be(uploadText);
        }

        [Fact]
        public async Task DownloadBlob_ShouldReturnNull_IfBlobNotAvailable()
        {
            // Arrange
            var repository = await SetupRepositoryAsync();

            // Act
            var result = await repository.DownloadBlobAsync(containerName, blobUrl);

            // Assert
            result.Should().BeNull();
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
