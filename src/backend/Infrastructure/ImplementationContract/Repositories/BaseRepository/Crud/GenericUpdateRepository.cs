using Application.Contracts.Localization;
using Application.Contracts.Repositories.BaseRepository.CRUD;
using Application.Extensions.ResultPattern;
using Application.Localization;
using Domain.Common;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Repositories.BaseRepository.Crud;

public class GenericUpdateRepository<T>(
    DataContext dbContext,
    ITextLocalizer localizer) : IGenericUpdateRepository<T> where T : BaseEntity
{
    public async Task<Result<int>> UpdateAsync(T value)
    {
        try
        {
            T? existing = await dbContext.Set<T>().AsTracking().FirstOrDefaultAsync(x => x.Id == value.Id && !x.IsDeleted);
            if (existing == null)
                return Result<int>.Failure(ErrorFactory.NotFound(localizer));

            dbContext.Entry(existing).CurrentValues.SetValues(value);

            if (dbContext.Entry(existing).State == EntityState.Unchanged)
                return Result<int>.Success(1);

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
