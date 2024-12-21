using Application.Extensions.ResultPattern;
using Domain.Common;

namespace Application.Contracts.Repositories.BaseRepository.CRUD;

public interface IGenericAddRepository<T> where T : BaseEntity
{
    Task<Result<int>> AddAsync(T entity);
    Task<Result<int>> AddRangeAsync(IEnumerable<T> entities);
}