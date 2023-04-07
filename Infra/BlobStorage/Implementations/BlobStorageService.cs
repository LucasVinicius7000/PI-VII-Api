using LocalStore.Infra.BlobStorage.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace LocalStore.Infra.BlobStorage.Implementations
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IConfiguration _configuration;
        private BlobServiceClient Client { get; }
        private BlobContainerClient Container { get; }

        public BlobStorageService(IConfiguration configuration)
        {
            _configuration = configuration;

            Client = new BlobServiceClient(
                _configuration.GetConnectionString("AzureStorage"));

            Container = Client.GetBlobContainerClient(
                _configuration.GetValue<string>("AzureBlobContainer"));
        }

        public async Task<string> UploadFile(string fileName, string fileExtension, Stream content)
        {
            try
            {
                BlobClient blobClient = Container.GetBlobClient(fileName + "." + fileExtension);
                var result = await blobClient.UploadAsync(content, new BlobHttpHeaders { ContentType = "image/" + fileExtension });
                return Container.GetBlobClient(fileName + "." + fileExtension).Uri.ToString();

            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao realizar o upload do arquivo " + fileName + fileExtension + " : " + ex.Message);
            }
        }

    }
}