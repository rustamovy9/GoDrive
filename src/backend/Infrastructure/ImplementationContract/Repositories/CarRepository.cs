using Application.Contracts.Localization;
using Application.Contracts.Repositories;
using Application.Extensions.ResultPattern;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.DataAccess;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Repositories;

public class CarRepository(DataContext dbContext, ITextLocalizer localizer)
    : GenericRepository<Car>(dbContext, localizer), ICarRepository
{
    private readonly DataContext _dbContext = dbContext;

    public async Task<Result<IEnumerable<Car>>> GetAvailableCarsAsync()
    {
        try
        {
            var data = await _dbContext.Cars
                .AsNoTracking()
                .Include(c => c.RentalCompany)
                .Include(c => c.Reviews)
                .Include(c => c.CarPrices)
                .Include(c => c.Location)
                .Where(c => c.IsActive && !c.IsDeleted && c.CarStatus == CarStatus.Available).ToListAsync();

            return Result<IEnumerable<Car>>.Success(data);

        }
        catch (Exception e)
        {
            return Result<IEnumerable<Car>>.Failure(Error.InternalServerError(e.Message));
        }
    }

    public async Task<int> CountCars()
    {
        return await dbContext.Cars.CountAsync();
    }
}
