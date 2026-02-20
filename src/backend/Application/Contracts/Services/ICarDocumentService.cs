using Application.DTO_s;
using Application.Extensions.ResultPattern;

namespace Application.Contracts.Services;

public interface ICarDocumentService
{
    Task<Result<IEnumerable<CarDocumentReadInfo>>> GetByCarIdAsync(int carId);

    Task<BaseResult> CreateAsync(CarDocumentCreateInfo createInfo);

    Task<BaseResult> UpdateStatusAsync(int id, int adminId, CarDocumentUpdateInfo updateInfo);

    Task<BaseResult> UpdateAsync(int id, CarDocumentCreateInfo updateInfo, int currentUserId);

    Task<BaseResult> DeleteAsync(int id, int currentUserId, bool isAdmin);

    Task<Result<(byte[] FileBytes, string FileName)>> DownloadAsync(int id, int currentUserId, bool isAdmin);
}