using Application.Extensions.ResultPattern;
using Domain.Common;

namespace Application.Contracts.Repositories.BaseRepository.CRUD;

public interface IGenericDeleteRepository<T> where T : BaseEntity
{
    Task<Result<int>> DeleteAsync(int id);
    Task<Result<int>> DeleteAsync(T value);
}