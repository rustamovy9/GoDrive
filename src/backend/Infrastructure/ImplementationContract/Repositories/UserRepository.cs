using Application.Contracts.Repositories;
using Application.Extensions.ResultPattern;
using Domain.Common;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.DataAccess;
using Infrastructure.ImplementationContract.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Repositories;

public class UserRepository(DataContext dbContext)
    : GenericRepository<User>(dbContext), IUserRepository
{
    private readonly DataContext _dbContext = dbContext;

    public async Task<Result<IEnumerable<User>>> GetAdminsAsync()
    {
        try
        {
            var res = await _dbContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Where(u => u.UserRoles
                    .Any(ur => ur.Role.Name == DefaultRoles.Admin))
                .ToListAsync();

            return Result<IEnumerable<User>>.Success(res);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Result<IEnumerable<User>>.Failure(Error.InternalServerError());
        }
    }

    public async Task<Result<bool>> IsAdminAsync(int adminId)
    {
        try
        {
            var res = await _dbContext.Users
                .Where(u => u.Id == adminId)
                .AnyAsync(u => u.UserRoles
                    .Any(r => r.Role.Name == DefaultRoles.Admin));

            return Result<bool>.Success(res);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Result<bool>.Failure(Error.InternalServerError());
        }
    }
}