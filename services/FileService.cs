using Core.Errors;
using Core.Exceptions;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;

    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
    private readonly string[] _allowedTypes = { "image/jpeg", "image/png", "image/webp" };

    private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folder)
    {
        Validate(file);

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var fileName = $"{Guid.NewGuid()}{extension}";

        var uploadPath = Path.Combine(_env.WebRootPath, folder);

        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        var fullPath = Path.Combine(uploadPath, fileName);

        using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/{folder}/{fileName}";
    }

    private void Validate(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new AppException(ErrorCode.BadRequest, ErrorMessage.ImageInvalid);

        if (file.Length > MaxFileSize)
            throw new AppException(ErrorCode.BadRequest, ErrorMessage.FileExceedsMaximumSize);

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!_allowedExtensions.Contains(extension))
            throw new AppException(ErrorCode.BadRequest, ErrorMessage.InvalidFileExtension);

        if (!_allowedTypes.Contains(file.ContentType.ToLower()))
            throw new AppException(ErrorCode.BadRequest, ErrorMessage.InvalidFileType);
    }

    public Task DeleteFileAsync(string fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
            return Task.CompletedTask;

        var fullPath = Path.Combine(_env.WebRootPath, fileUrl.TrimStart('/'));

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.CompletedTask;
    }

    public Task<bool> FileExistsAsync(string fileUrl)
    {
        var fullPath = Path.Combine(_env.WebRootPath, fileUrl.TrimStart('/'));
        return Task.FromResult(File.Exists(fullPath));
    }
}