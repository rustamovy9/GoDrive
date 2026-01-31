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
using Infrastructure.Extensions;

namespace Infrastructure.ImplementationContract.Services;

public class ReviewService(IReviewRepository repository) : IReviewService
{
    public async Task<Result<PagedResponse<IEnumerable<ReviewReadInfo>>>> GetAllAsync(ReviewFilter filter)
    {
        return await Task.Run(() =>
        {
            Expression<Func<Review, bool>> filterExpression = spec =>
                (filter.UserId == null || spec.UserId == filter.UserId) &&
                (filter.CarId == null || spec.CarId == filter.CarId) &&
                (filter.Rating == null || spec.Rating == filter.Rating);

            Result<IQueryable<Review>> request = repository.Find(filterExpression);

            if (!request.IsSuccess)
                return Result<PagedResponse<IEnumerable<ReviewReadInfo>>>.Failure(request.Error);

            List<ReviewReadInfo> query = request.Value!.Select(x => x.ToRead()).ToList();

            int count = query.Count;

            IEnumerable<ReviewReadInfo> spec = query.Page(filter.PageNumber, filter.PageSize);

            PagedResponse<IEnumerable<ReviewReadInfo>> res =
                PagedResponse<IEnumerable<ReviewReadInfo>>.Create(filter.PageNumber, filter.PageSize, count, spec);

            return Result<PagedResponse<IEnumerable<ReviewReadInfo>>>.Success(res);
        });
    }

    public async Task<Result<ReviewReadInfo>> GetByIdAsync(int id)
    {
        Result<Review?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess) return Result<ReviewReadInfo>.Failure(res.Error);

        return Result<ReviewReadInfo>.Success(res.Value!.ToRead());
    }

    public async Task<BaseResult> CreateAsync(int userId,ReviewCreateInfo createInfo)
    {
        Result<int> res = await repository.AddAsync(createInfo.ToEntity(userId));

        return res.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(res.Error);
    }

    public async Task<BaseResult> UpdateAsync(int id, ReviewUpdateInfo updateInfo)
    {
        Result<Review?> res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess) return BaseResult.Failure(Error.NotFound());

        Result<int> result = await repository.UpdateAsync(res.Value!.ToEntity(updateInfo));

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }

    public async Task<BaseResult> DeleteAsync(int id)
    {
        Result<Review?> res = await repository.GetByIdAsync(id);
        if (!res.IsSuccess) return BaseResult.Failure(Error.NotFound());

        Result<int> result = await repository.DeleteAsync(id);

        return result.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(result.Error);
    }
}
