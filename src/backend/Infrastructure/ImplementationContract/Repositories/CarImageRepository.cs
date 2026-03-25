using System.Data.Common;
using Application.Contracts.Repositories;
using Application.Extensions.ResultPattern;
using Domain.Common;
using Domain.Entities;
using Infrastructure.DataAccess;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;

namespace Infrastructure.ImplementationContract.Repositories;

public class CarImageRepository(DataContext dbContext) 
    : GenericRepository<CarImage>(dbContext), ICarImageRepository
{
    private readonly DataContext _dbContext = dbContext;

    public async Task<BaseResult> UpdateRangeAsync(IEnumerable<CarImage> entities)
    {
        try
        {
            _dbContext.CarImages.UpdateRange(entities);
            await _dbContext.SaveChangesAsync();
            return BaseResult.Success();
        }
        catch (Exception e)
        {
            return BaseResult.Failure(Error.InternalServerError());
        }
    }
}