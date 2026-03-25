using Application.Contracts.Repositories.BaseRepository.CRUD;
using Application.Extensions.ResultPattern;
using Domain.Common;
using Infrastructure.DataAccess;

namespace Infrastructure.ImplementationContract.Repositories.BaseRepository.Crud;

public class GenericAddRepository<T>(DataContext dbContext) : IGenericAddRepository<T> where T : BaseEntity
{
    public async Task<Result<int>> AddAsync(T entity)
    {
        try
        {
            await dbContext.Set<T>().AddAsync(entity);
            int res = await dbContext.SaveChangesAsync();
            return res > 0
                ? Result<int>.Success(res)
                : Result<int>.Failure(Error.InternalServerError());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Result<int>.Failure(Error.InternalServerError());
        }
    }

    public async Task<Result<int>> AddRangeAsync(IEnumerable<T> entities)
    {
        try
        {
            await dbContext.Set<T>().AddRangeAsync(entities);
            int res = await dbContext.SaveChangesAsync();
            return res > 0
                ? Result<int>.Success(res)
                : Result<int>.Failure(Error.InternalServerError());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Result<int>.Failure(Error.InternalServerError());
        }
    }
}