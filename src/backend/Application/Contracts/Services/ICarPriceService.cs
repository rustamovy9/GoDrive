using Application.DTO_s;
using Application.Extensions.ResultPattern;

namespace Application.Contracts.Services;

public interface ICarPriceService
{
    Task<Result<CarPriceReadInfo>> GetByCarIdAsync(int carId);
    Task<BaseResult> CreateAsync(CarPriceCreateInfo createInfo,int currentUserId,bool isAdmin);
    Task<BaseResult> UpdateAsync(int id,CarPriceUpdateInfo updateInfo,int currentUserId,bool isAdmin);
    Task<BaseResult> DeleteAsync(int id,int currentUserId,bool isAdmin);
}