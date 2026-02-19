using Application.Contracts.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.ImplementationContract.Services;

public class FileService(IWebHostEnvironment hostEnvironment) : IFileService
{
    private const long MaxFileSize = 50 * 1024 * 1024;

    private readonly HashSet<string> _allowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        { ".jpg", ".png", ".jpeg", ".mp4", ".pdf" };

    public async Task<string> CreateFile(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
            throw new InvalidOperationException("File is empty");

        var extension = Path.GetExtension(file.FileName);

        if (!_allowedExtensions.Contains(extension))
            throw new InvalidOperationException("Invalid file type.");

        if (file.Length > MaxFileSize)
            throw new InvalidOperationException("File size exceeds limit");

        var fileName = $"{Guid.NewGuid()}{extension}";
        var fullPath = GetFullPath(fileName, folder);

        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);

        return fileName; 
    }


    public bool DeleteFile(string fileName, string folder)
    {
        var fullPath = GetFullPath(fileName, folder);

        if (!File.Exists(fullPath))
            return false;

        File.Delete(fullPath);
        return true;
    }

    public async Task<(byte[] FileBytes, string FileName)> GetFileAsync(string fileName, string folder)
    {
        var fullPath = GetFullPath(fileName, folder);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException("File not found");

        var bytes = await File.ReadAllBytesAsync(fullPath);

        return (bytes, fileName);
    }

    public bool FileExists(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;

        return File.Exists(path);
    }


    private string GetFullPath(string fileName, string folder)
    {
        var root = hostEnvironment.WebRootPath;

        if (string.IsNullOrEmpty(root))
            root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        var folderPath = Path.Combine(root, folder);

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        return Path.Combine(folderPath, fileName);
    }
}