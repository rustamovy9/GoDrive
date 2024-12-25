using Application.Contracts.Services;
using Application.DTO_s;
using Application.Extensions.Mappers;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.DataAccess;
using Infrastructure.Extensions.Algorithms;
using Infrastructure.Extensions.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Services;

public class AuthService(DataContext dbContext, IAuthenticationService service) : IAuthService
{
    public async Task<Tuple<string, bool>> LoginAsync(LoginRequest request)
    {
        User? user = await dbContext.Users
            .FirstOrDefaultAsync(x => (x.UserName == request.UserName
                                       || x.Email == request.UserName
                                       || x.PhoneNumber == request.UserName)
                                      && x.PasswordHash == HashAlgorithms
                                          .ConvertToHash(request.Password));
        if (user is null)
            return Tuple.Create("Invalid username or password", false);

        await dbContext.SaveChangesAsync();
        return Tuple.Create(await service.GenerateTokenAsync(user), true);
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        bool conflict = await dbContext.Users.AnyAsync(
            x => x.UserName == request.UserName
                 || x.Email == request.EmailAddress
                 || x.PhoneNumber == request.PhoneNumber);
        if (conflict) return false;

        User newUser = request.ToEntity();

        Role? existingRole = await dbContext.Roles
            .FirstOrDefaultAsync(x => x.Name == DefaultRoles.User);
        if (existingRole is null) return false;
        
        await dbContext.Users.AddAsync(newUser);
        await dbContext.UserRoles.AddAsync(new()
            { UserId = newUser.Id, RoleId = existingRole.Id });

        int res = await dbContext.SaveChangesAsync();
        return res != 0;
    }

    public async Task<bool> DeleteAccountAsync(int userId)
    {
        User? user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null) return false;

        user.ToDelete();
        int res = await dbContext.SaveChangesAsync();
        return res != 0;
    }

    public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequest request)
    {
        User? user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null) return false;

        bool checkPassword = user.PasswordHash == HashAlgorithms.ConvertToHash(request.OldPassword);
        if (!checkPassword) return false;

        user.PasswordHash = HashAlgorithms.ConvertToHash(request.NewPassword);

        int res = await dbContext.SaveChangesAsync();
        return res != 0;
    }
}