using Application.Contracts.Repositories;
using Application.Extensions.ResultPattern;
using Domain.Common;
using Domain.Entities;
using Infrastructure.DataAccess;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Repositories;

public class RentalCompanyRepository(DataContext dbContext)
    : GenericRepository<RentalCompany>(dbContext), IRentalCompanyRepository
{
    private readonly DataContext _dbContext = dbContext;

    public async Task<Result<RentalCompany?>> GetByIdAndIncludeLocationAsync(int id)
    {
        try
        {
            RentalCompany? res = await _dbContext.Set<RentalCompany>()
                .Include(x=>x.Location)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            return res != null
                ? Result<RentalCompany?>.Success(res)
                : Result<RentalCompany?>.Failure(Error.NotFound());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Result<RentalCompany?>.Failure(Error.InternalServerError());
        }
    }
}