using System.Linq.Expressions;
using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.DTO_s;
using Application.Extensions.Mappers;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Application.Filters;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Extensions;

namespace Infrastructure.ImplementationContract.Services;

public class BookingService(IBookingRepository repository) : IBookingService
{
    public async Task<Result<PagedResponse<IEnumerable<BookingReadInfo>>>> GetAllAsync(BookingFilter filter)
    {
        return await Task.Run(() =>
        {
            Expression<Func<Booking, bool>> filterExpression = spec =>
                (filter.Status == null || spec.Status == filter.Status) &&
                (filter.UserId == null || spec.UserId == filter.UserId) &&
                (filter.CarId == null || spec.CarId == filter.CarId) &&
                (filter.StartDate == null || spec.StartDateTime >= filter.StartDate) &&
                (filter.EndDate == null || spec.EndDateTime <= filter.EndDate);

            Result<IQueryable<Booking>> request = repository
                .Find(filterExpression);

            if (!request.IsSuccess)
                return Result<PagedResponse<IEnumerable<BookingReadInfo>>>.Failure(request.Error);

            List<BookingReadInfo> query = request.Value!.Select(x => x.ToRead()).ToList();

            int count = query.Count;

            IEnumerable<BookingReadInfo> spec =
                query.Page(filter.PageNumber, filter.PageSize);

            PagedResponse<IEnumerable<BookingReadInfo>> res =
                PagedResponse<IEnumerable<BookingReadInfo>>.Create(filter.PageNumber, filter.PageSize, count, spec);

            return Result<PagedResponse<IEnumerable<BookingReadInfo>>>.Success(res);
        });
    }

    public async Task<Result<BookingReadInfo>> GetByIdAsync(int id)
    {
        Result<Booking?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess) return Result<BookingReadInfo>.Failure(res.Error);

        return Result<BookingReadInfo>.Success(res.Value!.ToRead());
    }

    public async Task<BaseResult> CreateAsync(BookingCreateInfo createInfo)
    {
        bool conflict = (await repository.GetAllAsync()).Value!.Any(b => b.CarId == createInfo.CarId &&
                                                                         b.StartDateTime < createInfo.EndDateTime &&
                                                                         createInfo.StartDateTime < b.EndDateTime);

        if (conflict) return BaseResult.Failure(Error.Conflict());

        Result<int> res = await repository.AddAsync(createInfo.ToEntity());

        return res.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(res.Error);
    }

    public async Task<BaseResult> UpdateAsync(int id, BookingUpdateInfo updateInfo)
    {
        Result<Booking?> res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess) return BaseResult.Failure(Error.NotFound());

        bool conflict = (await repository.GetAllAsync()).Value!.Any(b => b.CarId == updateInfo.CarId &&
                                                                         b.StartDateTime < updateInfo.EndDateTime &&
                                                                         updateInfo.StartDateTime < b.EndDateTime);
        if (conflict) return BaseResult.Failure(Error.Conflict());

        Result<int> result = await repository.UpdateAsync(res.Value!.ToEntity(updateInfo));

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    public async Task<BaseResult> DeleteAsync(int id)
    {
        Result<Booking?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess) return BaseResult.Failure(Error.NotFound());
        
        
        Result<int> result = await repository.DeleteAsync(id);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }
}