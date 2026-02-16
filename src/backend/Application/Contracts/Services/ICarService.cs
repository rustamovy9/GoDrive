using Application.DTO_s;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Application.Filters;
using Domain.Enums;

namespace Application.Contracts.Services;

public interface ICarService
{
    Task<Result<PagedResponse<IEnumerable<CarReadInfo>>>> GetAllAsync(CarFilter filter,string role,int userId);
    Task<Result<CarReadInfo>> GetByIdAsync(int id,int currentUserId,bool isAdmin);
    Task<BaseResult> CreateAsync(CarCreateInfo createInfo,int ownerId);
    Task<BaseResult> UpdateAsync(int id,CarUpdateInfo updateInfo,int currentUserId,bool isAdmin);
    Task<BaseResult> DeleteAsync(int id,int currentUserId,bool isAdmin);
    Task<BaseResult> UpdateStatusAsync(int id, CarUpdateStatusInfo status,bool isAdmin);
}