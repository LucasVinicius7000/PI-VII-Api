namespace LocalStore.Infra.Services.BlobStorage.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadImageFile(string fileName, string fileExtension, Stream content);
        Task<string> UploadFile(string fileName, string fileExtension, Stream content);
    }
}
