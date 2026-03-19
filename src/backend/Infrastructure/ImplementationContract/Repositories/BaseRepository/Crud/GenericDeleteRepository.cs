using Application.Contracts.Localization;
using Application.Contracts.Repositories.BaseRepository.CRUD;
using Application.Extensions.ResultPattern;
using Application.Localization;
using Domain.Common;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Repositories.BaseRepository.Crud;

public class GenericDeleteRepository<T>(
    DataContext dbContext,
    ITextLocalizer localizer) : IGenericDeleteRepository<T> where T : BaseEntity
{
    public async Task<Result<int>> DeleteAsync(int id)
    {
        try
        {
            T? entity = await dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                return Result<int>.Failure(ErrorFactory.NotFound(localizer));

            dbContext.Set<T>().Update((T)entity.ToDelete());
            int res = await dbContext.SaveChangesAsync();
            return res > 0
                ? Result<int>.Success(res)
                : Result<int>.Failure(ErrorFactory.InternalServerError(localizer));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Result<int>.Failure(ErrorFactory.InternalServerError(localizer));
        }
    }

    public async Task<Result<int>> DeleteAsync(T value)
    {
        try
        {
            T? entity = await dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == value.Id && !x.IsDeleted);
            if (entity == null)
                return Result<int>.Failure(ErrorFactory.NotFound(localizer));

            dbContext.Set<T>().Update((T)entity.ToDelete());
            int res = await dbContext.SaveChangesAsync();
            return res > 0
                ? Result<int>.Success(res)
                : Result<int>.Failure(ErrorFactory.InternalServerError(localizer));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Result<int>.Failure(ErrorFactory.InternalServerError(localizer));
        }
    }
}
