using Application.Contracts.Repositories.BaseRepository.CRUD;
using Application.Extensions.ResultPattern;
using Domain.Common;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Repositories.BaseRepository.Crud;

public class GenericUpdateRepository<T>(DataContext dbContext) : IGenericUpdateRepository<T> where T : BaseEntity
{
    public async Task<Result<int>> UpdateAsync(T value)
    {
        try
        {
            T? existing = await dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == value.Id);
            if (existing == null)
                return Result<int>.Failure(Error.NotFound());

            dbContext.Entry(existing).CurrentValues.SetValues(value);

            if (dbContext.Entry(existing).State == EntityState.Unchanged)
                return Result<int>.Success(1);

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