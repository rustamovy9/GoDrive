using Application.Contracts.Repositories.BaseRepository;
using Application.Extensions.ResultPattern;
using Domain.Entities;

namespace Application.Contracts.Repositories;

public interface ICarRepository : IGenericRepository<Car>
{
    Task<Result<IEnumerable<Car>>> GetAvailableCarsAsync();
}