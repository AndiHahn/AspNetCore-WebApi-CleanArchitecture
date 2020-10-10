using Azure.Storage.Blobs;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.Interfaces.BlobStorage
{
    public interface IBlobClientFactory
    {
        Task<BlobClient> CreateClientAsync(string fileUrl);
    }
}