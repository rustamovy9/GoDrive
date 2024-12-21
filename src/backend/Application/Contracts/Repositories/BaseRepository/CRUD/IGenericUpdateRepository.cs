using Application.Extensions.ResultPattern;
using Domain.Common;

namespace Application.Contracts.Repositories.BaseRepository.CRUD;

public interface IGenericUpdateRepository<T> where T : BaseEntity
{
    Task<Result<int>> UpdateAsync(T value);
}