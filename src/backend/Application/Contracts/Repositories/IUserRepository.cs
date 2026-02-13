using Application.Contracts.Repositories.BaseRepository;
using Application.Extensions.ResultPattern;
using Domain.Entities;

namespace Application.Contracts.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<Result<IEnumerable<User>>> GetAdminsAsync();
    Task<Result<bool>> IsAdminAsync(int adminId);
}