using Microsoft.AspNetCore.Http;

namespace Application.Contracts.Services;

public interface IFileService
{
    string GetFileUrl(string fileName,string folder);
    Task<string> CreateFile(IFormFile file,string folder);
    Task DeleteFile(string file,string folder);
    Task<(byte[] FileBytes, string FileName)> DownloadAsync(string fileName, string bucket);
}