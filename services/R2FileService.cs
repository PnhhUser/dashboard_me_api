using Amazon.S3;
using Amazon.S3.Model;

public class R2FileService : IFileService
{
    private readonly IAmazonS3 _s3;
    private readonly IConfiguration _config;
    private readonly string _bucket;
    private readonly string _publicUrl;

    public R2FileService(IAmazonS3 s3, IConfiguration config)
    {
        _s3 = s3;
        _config = config;
        _bucket = config["R2:BucketName"]!;
        _publicUrl = config["R2:PublicUrl"]!;
    }

    public async Task<string> SaveFileAsync(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
            throw new Exception("File không hợp lệ");

        // 🔥 validate basic
        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType))
            throw new Exception("Chỉ cho phép ảnh (jpg, png, webp)");

        // 🔥 rename file tránh trùng
        var ext = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{ext}";

        folder = string.IsNullOrEmpty(folder) ? "uploads" : folder;
        var key = $"{folder}/{fileName}";

        // 🔥 QUAN TRỌNG: chuyển sang MemoryStream để tránh streaming AWS4
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        ms.Position = 0;

        var request = new PutObjectRequest
        {
            BucketName = _bucket,
            Key = key,
            InputStream = ms,
            ContentType = file.ContentType,
            AutoCloseStream = true,
            UseChunkEncoding = false
        };

        await _s3.PutObjectAsync(request);

        // 🔥 trả URL public
        return $"{_publicUrl}/{key}";
    }

    public async Task DeleteFileAsync(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
            return;

        // 🔥 lấy key từ URL
        var key = fileUrl.Replace($"{_publicUrl}/", "");

        var request = new DeleteObjectRequest
        {
            BucketName = _bucket,
            Key = key
        };

        await _s3.DeleteObjectAsync(request);
    }

    public async Task<bool> FileExistsAsync(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
            return false;

        try
        {
            var key = fileUrl.Replace($"{_publicUrl}/", "");

            var request = new GetObjectMetadataRequest
            {
                BucketName = _bucket,
                Key = key
            };

            var response = await _s3.GetObjectMetadataAsync(request);

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
        catch
        {
            return false;
        }
    }
}