﻿using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Core.Exceptions;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Models;
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
            Assert.NotNull(downloadedBlob);
            Assert.Equal(blobEntity.ContentType, downloadedBlob.ContentType);
            using StreamReader reader = new StreamReader(downloadedBlob.Content);
            string downloadedText = reader.ReadToEnd();
            Assert.Equal(uploadText, downloadedText);
        }

        [Fact]
        public async Task DownloadBlob_ShouldThrowException_IfBlobNotAvailable()
        {
            // Arrange
            var repository = await SetupRepositoryAsync();

            // Act && Assert
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
