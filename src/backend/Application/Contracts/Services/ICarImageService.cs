using Application.DTO_s;
using Application.Extensions.ResultPattern;

namespace Application.Contracts.Services;

public interface ICarImageService
{
    Task<Result<IEnumerable<CarImageReadInfo>>> GetByCarIdAsync(int carId,int currentUserId, bool isAdmin);

    Task<BaseResult> CreateAsync(CarImageCreateInfo createInfo,int currentUserId);

    Task<BaseResult> DeleteAsync(int id,int currentUserId,bool isAdmin);
    Task<BaseResult> SetMainAsync(int imageId, int currentUserId);

    Task<Result<(byte[] FileBytes, string FileName)>> DownloadAsync(int id,int currentUserId,bool isAdmin);
}