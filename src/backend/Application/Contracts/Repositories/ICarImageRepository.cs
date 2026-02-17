using Application.Contracts.Repositories.BaseRepository;
using Application.Extensions.ResultPattern;
using Domain.Entities;

namespace Application.Contracts.Repositories;

public interface ICarImageRepository : IGenericRepository<CarImage>
{
    Task<BaseResult> UpdateRangeAsync(IEnumerable<CarImage> entities);
}