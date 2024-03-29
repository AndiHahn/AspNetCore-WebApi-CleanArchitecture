﻿using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.Cosmos.Table;

namespace CleanArchitecture.Shopping.Infrastructure.AzureStorage
{
    internal interface IAzureStorageClientFactory
    {
        Task<CloudTableClient> GetOrCreateTableClientAsync(string tableName);

        Task CreateTableIfNotExistsAsync(string tableName);

        CloudTableClient GetTableClient();

        Task<BlobClient> GetOrCreateBlobClientAsync(string containerName, string blobName);

        Task CreateBlobContainerIfNotExistsAsync(string containerName);

        BlobClient GetBlobClient(string containerName, string blobName);
    }
}
