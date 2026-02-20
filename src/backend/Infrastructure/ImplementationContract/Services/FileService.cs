using Application.Contracts.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.ImplementationContract.Services;

public class FileService : IFileService
{
    private const long MaxFileSize = 50 * 1024 * 1024;

    private readonly HashSet<string> _allowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        { ".jpg", ".png", ".jpeg", ".mp4", ".pdf" };

    
    private readonly Supabase.Client _client;
    
    public FileService(IConfiguration config) 
    {
        var url = config["Supabase:Url"];
        var key = config["Supabase:Key"];

        if (string.IsNullOrWhiteSpace(url))
            throw new Exception("Supabase Url is missing");

        if (string.IsNullOrWhiteSpace(key))
            throw new Exception("Supabase Key is missing");

        _client = new Supabase.Client(url, key);
    }
    
    
    public async Task<string> CreateFile(IFormFile file, string bucket)
    {
        if (file == null || file.Length == 0)
            throw new InvalidOperationException("File is empty");

        var extension = Path.GetExtension(file.FileName);

        var fileName = $"{Guid.NewGuid()}{extension}";

        using var memoryStream = new MemoryStream();

        await file.CopyToAsync(memoryStream);

        var bytes = memoryStream.ToArray();

        var storage = _client.Storage.From(bucket);

        await storage.Upload(bytes, fileName);          

        return fileName;
    }


    public async Task DeleteFile(string fileName, string bucket)
    {
        var storage = _client.Storage.From(bucket);
        await storage.Remove(fileName);
    }

    public async Task<(byte[] FileBytes, string FileName)> DownloadAsync(string fileName, string bucket)
    {
        var storage = _client.Storage.From(bucket);

        var bytes = await storage.Download(fileName,null);

        return (bytes, fileName);
    }
}