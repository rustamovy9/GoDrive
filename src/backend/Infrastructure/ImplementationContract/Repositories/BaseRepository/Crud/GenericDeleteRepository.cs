using Application.Contracts.Repositories.BaseRepository.CRUD;
using Application.Extensions.ResultPattern;
using Domain.Common;
using Domain.Extensions;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Repositories.BaseRepository.Crud;

public class GenericDeleteRepository<T>(DataContext dbContext) : IGenericDeleteRepository<T> where T : BaseEntity
{
    public async Task<Result<int>> DeleteAsync(int id)
    {
        try
        {
            T? entity = await dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
                return Result<int>.Failure(Error.NotFound());

            dbContext.Set<T>().Update((T)entity.ToDelete());
            int res = await dbContext.SaveChangesAsync();
            return res > 0
                ? Result<int>.Success(res)
                : Result<int>.Failure(Error.InternalServerError());
        }
        catch (Exception ex)
        {
            return Result<int>.Failure(Error.InternalServerError(ex.Message));
        }
    }

    public async Task<Result<int>> DeleteAsync(T value)
    {
        try
        {
            T? entity = await dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == value.Id);
            if (entity == null)
                return Result<int>.Failure(Error.NotFound());

            dbContext.Set<T>().Update((T)entity.ToDelete());
            int res = await dbContext.SaveChangesAsync();
            return res > 0
                ? Result<int>.Success(res)
                : Result<int>.Failure(Error.InternalServerError());
        }
        catch (Exception ex)
        {
            return Result<int>.Failure(Error.InternalServerError(ex.Message));
        }
    }
}