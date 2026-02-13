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
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Services;

public class ReviewService(IReviewRepository repository,IBookingRepository bookingRepository) : IReviewService
{
    public async Task<Result<PagedResponse<IEnumerable<ReviewReadInfo>>>> GetAllAsync(ReviewFilter filter)
    {
            Expression<Func<Review, bool>> filterExpression = spec =>
                (filter.UserId == null || spec.UserId == filter.UserId) &&
                (filter.CarId == null || spec.CarId == filter.CarId) &&
                (filter.Rating == null || spec.Rating == filter.Rating) &&
                (filter.FromDate == null || spec.CreatedAt >= filter.FromDate) &&
                (filter.ToDate == null || spec.CreatedAt <= filter.ToDate);
            
            
            Result<IQueryable<Review>> request = repository.Find(filterExpression);

            if (!request.IsSuccess)
                return Result<PagedResponse<IEnumerable<ReviewReadInfo>>>.Failure(request.Error);

            var query = request.Value!.AsNoTracking();
            
            int count = await query.CountAsync();

            IEnumerable<ReviewReadInfo> data = await query
                .OrderByDescending(x=>x.CreatedAt)
                .Skip((filter.PageNumber-1)*filter.PageSize)
                .Take(filter.PageSize)
                .Select(x=>x.ToRead())
                .ToListAsync();

            PagedResponse<IEnumerable<ReviewReadInfo>> res =
                PagedResponse<IEnumerable<ReviewReadInfo>>.Create(filter.PageNumber, filter.PageSize, count, data);

            return Result<PagedResponse<IEnumerable<ReviewReadInfo>>>.Success(res);
    }

    public async Task<Result<IEnumerable<ReviewReadInfo>>> GetByCarIdAsync(int carId)
    {
        var res = repository.Find(x => x.CarId == carId);

        if (!res.IsSuccess)
            return Result<IEnumerable<ReviewReadInfo>>.Failure(res.Error);

        var data = await res.Value!
            .AsNoTracking()
            .OrderByDescending(x=>x.CreatedAt)
            .Select(x=>x.ToRead())
            .ToListAsync();

        return Result<IEnumerable<ReviewReadInfo>>.Success(data);
    }

    public async Task<Result<ReviewReadInfo>> GetByIdAsync(int id)
    {
        Result<Review?> res = await repository.GetByIdAsync(id);
        
        if (!res.IsSuccess || res.Value is null) return Result<ReviewReadInfo>.Failure(Error.NotFound("Review not found"));

        return Result<ReviewReadInfo>.Success(res.Value.ToRead());
    }

    public async Task<BaseResult> CreateAsync(int userId,ReviewCreateInfo createInfo)
    {
        var hasCompletedBooking = bookingRepository
            .Find(b => b.UserId == userId &&
                       b.CarId == createInfo.CarId &&
                       b.BookingStatus == BookingStatus.Completed);

        if (!hasCompletedBooking.IsSuccess ||
            !await hasCompletedBooking.Value!.AnyAsync())
        {
            return BaseResult.Failure(Error.BadRequest("You can leave review only after completed booking"));
        }
        
        var exists = repository.Find(x =>
            x.UserId == userId &&
            x.CarId == createInfo.CarId);
      

        if (exists.IsSuccess && await exists.Value!.AnyAsync())
            return BaseResult.Failure(
                Error.Conflict("You already reviewed this car."));
        
        Result<int> res = await repository.AddAsync(createInfo.ToEntity(userId));

        return res.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(res.Error);
    }

    public async Task<BaseResult> UpdateAsync(int id, ReviewUpdateInfo updateInfo)
    {
        Result<Review?> res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess || res.Value is null) return BaseResult.Failure(Error.NotFound("Review not found"));

        Result<int> result = await repository.UpdateAsync(res.Value!.ToEntity(updateInfo));

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    public async Task<BaseResult> DeleteAsync(int id)
    {
        Result<Review?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess || res.Value is null) return BaseResult.Failure(Error.NotFound("Review not found"));

        Result<int> result = await repository.DeleteAsync(id);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }
}
