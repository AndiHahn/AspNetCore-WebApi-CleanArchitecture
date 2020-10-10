using System.Threading.Tasks;
using Azure.Storage.Blobs;
using CleanArchitecture.Core.Interfaces.BlobStorage;
using CleanArchitecture.Core.Interfaces.Image;
using CleanArchitecture.Core.Services.Image;
using NSubstitute;
using Xunit;

namespace CleanArchitecture.UnitTests.ApplicationCore.Services
{
    public class ImageStorageServiceTests
    {
        private readonly IBlobClientFactory blobClientFactory;

        public ImageStorageServiceTests()
        {
            blobClientFactory = Substitute.For<IBlobClientFactory>();
        }

        [Fact]
        public async Task GetImageDownloadInfo_ShouldReturnNull_IfBlobClientDoesNotExist()
        {
            var mockedBlobClient = Substitute.For<BlobClient>();
            blobClientFactory.CreateClientAsync(Arg.Any<string>()).Returns(mockedBlobClient);
            var azureResponse = Substitute.For<Azure.Response<bool>>();
            azureResponse.Value.Returns(false);
            mockedBlobClient.Exists().Returns(azureResponse);
            var service = SetupService();

            var result = await service.GetImageDownloadInfoAsync("");
            Assert.Null(result);
        }

        [Fact]
        public async Task GetImageDownloadInfo_ShouldCallDownloadBlob_IfClientAvailable()
        {
            var mockedBlobClient = Substitute.For<BlobClient>();
            blobClientFactory.CreateClientAsync(Arg.Any<string>()).Returns(mockedBlobClient);
            var azureResponse = Substitute.For<Azure.Response<bool>>();
            azureResponse.Value.Returns(true);
            mockedBlobClient.ExistsAsync().Returns(azureResponse);
            var service = SetupService();

            await service.GetImageDownloadInfoAsync("");

            await mockedBlobClient.Received().DownloadAsync();
        }

        private IImageStorageService SetupService()
        {
            return new ImageStorageService(blobClientFactory);
        }
    }
}
