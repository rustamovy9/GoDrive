using Application.Contracts.Repositories;
using Application.Extensions.ResultPattern;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.DataAccess;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Repositories;

public class CarRepository(DataContext dbContext)
    : GenericRepository<Car>(dbContext), ICarRepository
{
    private readonly DataContext _dbContext = dbContext;

    public async Task<Result<IEnumerable<Car>>> GetAvailableCarsAsync()
    {
        try
        {
            var data = await _dbContext.Cars
                .Where(c => c.IsActive && !c.IsDeleted && c.CarStatus == CarStatus.Available).ToListAsync();

            return Result<IEnumerable<Car>>.Success(data);

        }
        catch (Exception e)
        {
            return Result<IEnumerable<Car>>.Failure(Error.InternalServerError(e.Message));
        }
    }
}