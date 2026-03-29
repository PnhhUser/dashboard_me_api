public interface IFileService
{
    Task<string> SaveFileAsync(IFormFile file, string folder);

    Task DeleteFileAsync(string fileUrl);

    // bool FileExists(string fileUrl);

    Task<bool> FileExistsAsync(string fileUrl);
}