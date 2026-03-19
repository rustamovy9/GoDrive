using System.Data.Common;
using Application.Contracts.Localization;
using Application.Contracts.Repositories;
using Application.Extensions.ResultPattern;
using Application.Localization;
using Domain.Common;
using Domain.Entities;
using Infrastructure.DataAccess;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;

namespace Infrastructure.ImplementationContract.Repositories;

public class CarImageRepository(DataContext dbContext, ITextLocalizer localizer)
    : GenericRepository<CarImage>(dbContext, localizer), ICarImageRepository
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
            return BaseResult.Failure(ErrorFactory.InternalServerError(localizer));
        }
    }
}
