using System.Linq.Expressions;
using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.Contracts.Localization;
using Application.DTO_s;
using Application.Extensions.Mappers;
using Application.Extensions.Responses.PagedResponse;
using Application.Extensions.ResultPattern;
using Application.Localization;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Services;

public class CategoryService(ICategoryRepository repository, ITextLocalizer localizer) : ICategoryService
{
    public async Task<BaseResult> CreateAsync(CategoryCreateInfo createInfo)
    {
        if (string.IsNullOrEmpty(createInfo.Name))
            return BaseResult.Failure(
                Error.BadRequest(localizer.Get(TextKeys.Errors.CategoryNameRequired)));

        var exists = await repository
            .Find(x => x.Name.ToLower() == createInfo.Name.ToLower())
            .Value!
            .AnyAsync();

        if (exists)
            return BaseResult.Failure(
                Error.Conflict(localizer.Get(TextKeys.Errors.CategoryExists)));

        var category = createInfo.ToEntity();

        var res = await repository.AddAsync(category);

        return res.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(res.Error);
    }

    public async Task<BaseResult> UpdateAsync(int id, CategoryUpdateInfo updateInfo)
    {
        var res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess || res.Value is null)
            return BaseResult.Failure(
                Error.NotFound(localizer.Get(TextKeys.Errors.CategoryNotFound)));

        var category = res.Value;

        if (!string.IsNullOrWhiteSpace(updateInfo.Name))
        {
            var exists = await repository
                .Find(x => x.Name.ToLower() == updateInfo.Name.ToLower() && x.Id != id)
                .Value!
                .AnyAsync();

            if (exists)
                return BaseResult.Failure(
                    Error.Conflict(localizer.Get(TextKeys.Errors.CategoryNameExists)));

            category.Name = updateInfo.Name;
        }

        category.UpdatedAt = DateTimeOffset.UtcNow;
        category.Version++;

        var updateResult = await repository.UpdateAsync(category);

        return updateResult.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(updateResult.Error);
    }

    public async Task<BaseResult> DeleteAsync(int id)
    {
        var res = await repository.DeleteAsync(id);

        return res.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(res.Error);
    }

    public async Task<Result<CategoryReadInfo>> GetByIdAsync(int id)
    {
        var res = await repository.GetByIdAsync(id);

        if (!res.IsSuccess || res.Value is null)
            return Result<CategoryReadInfo>.Failure(
                Error.NotFound(localizer.Get(TextKeys.Errors.CategoryNotFound)));

        return Result<CategoryReadInfo>.Success(res.Value.ToRead());
    }

    public async Task<Result<PagedResponse<IEnumerable<CategoryReadInfo>>>> GetAll(BaseFilter filter)
    {
        Expression<Func<Category, bool>> expression = c =>
            string.IsNullOrEmpty(filter.Search) ||
            c.Name.ToLower().Contains(filter.Search.ToLower());

        var result = repository.Find(expression);

        if (!result.IsSuccess)
            return Result<PagedResponse<IEnumerable<CategoryReadInfo>>>.Failure(result.Error);

        var list = await result.Value!
            .Select(x => x.ToRead())
            .ToListAsync();

        int count = list.Count;

        var paged = list.Page(filter.PageNumber, filter.PageSize);

        var response = PagedResponse<IEnumerable<CategoryReadInfo>>
            .Create(filter.PageSize, filter.PageNumber, count, paged);

        return Result<PagedResponse<IEnumerable<CategoryReadInfo>>>.Success(response);
    }
}
