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

        if (!_allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            throw new InvalidOperationException("Invalid file type.");

        if (file.Length > MaxFileSize)
            throw new InvalidOperationException("File size exceeds the maximum allowed size.");

        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        string folderPath = Path.Combine(hostEnvironment.WebRootPath ?? "wwwroot", folder);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string fullPath = Path.Combine(folderPath, fileName);

        try
        {
            await using FileStream stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return fileName;
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync(ex.Message);
            throw new InvalidOperationException("An error occurred while saving the file.");
        }
    }

    public bool DeleteFile(string file, string folder)
    {
        string folderPath = Path.Combine(hostEnvironment.WebRootPath ?? "wwwroot", folder);
        string fullPath = Path.Combine(folderPath, file);

        try
        {
            if (!Directory.Exists(folderPath)) 
                return false;
            
            File.Delete(fullPath);
            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            throw new InvalidOperationException("An error occurred while delete the file.");
        }
    }

    public async Task<(byte[] FileBytes, string FileName)> GetFileAsync(string fileName,string folder)
    {
        string folderPath = Path.Combine(hostEnvironment.WebRootPath ?? "wwwroot", folder);
        string fullPath = Path.Combine(folderPath, fileName);
        
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
}