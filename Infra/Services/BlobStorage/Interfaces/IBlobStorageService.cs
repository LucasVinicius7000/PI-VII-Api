namespace LocalStore.Infra.Services.BlobStorage.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadFile(string fileName, string fileExtension, Stream content);
    }
}
