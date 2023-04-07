namespace LocalStore.Infra.BlobStorage.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadFile(string fileName, string fileExtension, Stream content);
    }
}
