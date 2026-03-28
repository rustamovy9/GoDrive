using Application.Contracts.Repositories.BaseRepository;
using Application.Extensions.ResultPattern;
using Domain.Entities;

namespace Application.Contracts.Repositories;

public interface IRentalCompanyRepository : IGenericRepository<RentalCompany>
{
    Task<Result<RentalCompany?>> GetByIdAndIncludeLocationAsync(int id);
}