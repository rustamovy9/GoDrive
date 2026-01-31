using Application.Contracts.Services;
using Application.DTO_s;
using Application.Extensions.Algorithms;
using Application.Extensions.Mappers;
using Application.Extensions.ResultPattern;
using Domain.Common;
using Domain.Constants;
using Domain.Entities;
using Domain.Extensions;
using Infrastructure.DataAccess;
using Infrastructure.Extensions.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Services;

public class AuthService(DataContext dbContext, IAuthenticationService service) : IAuthService
{
    public async Task<Result<Tuple<string, bool>>> LoginAsync(LoginRequest request)
    {
        User? user = await dbContext.Users
            .FirstOrDefaultAsync(x => (x.UserName == request.UserName
                                       || x.Email == request.UserName
                                       || x.PhoneNumber == request.UserName)
                                      && x.PasswordHash == HashAlgorithms
                                          .ConvertToHash(request.Password));
        if (user is null)
            return Result<Tuple<string, bool>>.Failure(Error.BadRequest("Invalid username or password"));

        await dbContext.SaveChangesAsync();
        return Result<Tuple<string, bool>>.Success(Tuple.Create(await service.GenerateTokenAsync(user), true));
    }

    public async Task<BaseResult> RegisterAsync(RegisterRequest request)
    {
        Role? existingRole = await dbContext.Roles
            .FirstOrDefaultAsync(x => x.Name == DefaultRoles.User);
        if (existingRole is null) return BaseResult.Failure(Error.NotFound());

        if (request.Password.Length < 8)
            return BaseResult.Failure(Error.BadRequest("Password must be at least 8 characters long."));

        if (!request.Password.Equals(request.ConfirmPassword))
            return BaseResult.Failure(Error.BadRequest("Passwords do not match."));

        bool conflict = await dbContext.Users.AnyAsync(
            x => x.UserName == request.UserName
                 || x.Email == request.Email
                 || x.PhoneNumber == request.PhoneNumber);
        if (conflict) return BaseResult.Failure(Error.Conflict());

        User newUser = request.ToEntity();

        if (!IsValidDateOfBirth(request.DateOfBirth))
            return BaseResult.Failure(Error.BadRequest("Invalid date of birth provided."));


        await dbContext.Users.AddAsync(newUser);
        await dbContext.SaveChangesAsync();

        await dbContext.UserRoles.AddAsync(new()
            { UserId = newUser.Id, RoleId = existingRole.Id });

        await dbContext.SaveChangesAsync();
        return BaseResult.Success();
    }

    public async Task<BaseResult> DeleteAccountAsync(int userId)
    {
        User? user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null) return BaseResult.Failure(Error.NotFound());

        user.ToDelete();
        await dbContext.SaveChangesAsync();
        return BaseResult.Success();
    }
    
    public async Task<BaseResult> ChangePasswordAsync(int userId, ChangePasswordRequest request)
    {
        User? user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null) return BaseResult.Failure(Error.NotFound());


        bool checkPassword = user.PasswordHash == HashAlgorithms.ConvertToHash(request.OldPassword);
        if (!checkPassword) return BaseResult.Failure(Error.BadRequest("Password is incorrect"));

        if (!request.NewPassword.Equals(request.ConfirmPassword))
            return BaseResult.Failure(Error.BadRequest("Passwords do not match."));

        user.PasswordHash = HashAlgorithms.ConvertToHash(request.NewPassword);

        await dbContext.SaveChangesAsync();
        return BaseResult.Success();
    }

    private static bool IsValidDateOfBirth(DateTimeOffset dateOfBirth)
    {
        return dateOfBirth <= DateTime.UtcNow && dateOfBirth >= DateTime.UtcNow.AddYears(-150);
    }
}