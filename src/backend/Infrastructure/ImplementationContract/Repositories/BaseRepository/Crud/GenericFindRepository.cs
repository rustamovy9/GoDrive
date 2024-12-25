using System.Linq.Expressions;
using Application.Contracts.Repositories.BaseRepository.CRUD;
using Application.Extensions.ResultPattern;
using Domain.Common;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Repositories.BaseRepository.Crud;

public class GenericFindRepository<T>(DataContext dbContext) : IGenericFindRepository<T> where T : BaseEntity
{
    public Result<IQueryable<T>> Find(Expression<Func<T, bool>> expression)
    {
        try
        {
            return Result<IQueryable<T>>
                .Success(dbContext.Set<T>()
                    .Where(expression).AsQueryable());
        }
        catch (Exception ex)
        {
            return Result<IQueryable<T>>.Failure(Error.InternalServerError(ex.Message));
        }
    }

    public async Task<Result<IEnumerable<T>>> GetAllAsync()
    {
        try
        {
            return Result<IEnumerable<T>>
                .Success(await dbContext.Set<T>()
                    .ToListAsync());
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<T>>.Failure(Error.InternalServerError(ex.Message));
        }
    }

    public async Task<Result<T?>> GetByIdAsync(int id)
    {
        try
        {
            T? res = await dbContext.Set<T>()
                .FirstOrDefaultAsync(x => x.Id == id);
            return res != null
                ? Result<T?>.Success(res)
                : Result<T?>.Failure(Error.NotFound());
        }
        catch (Exception ex)
        {
            return Result<T?>.Failure(Error.InternalServerError(ex.Message));
        }
    }
}