using Application.Contracts.Localization;
using Application.Contracts.Services;
using Application.DTO_s;
using Application.Extensions.Algorithms;
using Application.Extensions.Mappers;
using Application.Extensions.ResultPattern;
using Application.Localization;
using Domain.Common;
using Domain.Constants;
using Domain.Entities;
using Domain.Extensions;
using Infrastructure.DataAccess;
using Infrastructure.Extensions.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Services;

public class AuthService(
    DataContext dbContext,
    IAuthenticationService service,
    ITextLocalizer localizer) : IAuthService
{
    public async Task<Result<Tuple<string, bool>>> LoginAsync(LoginRequest request)
    {
        User? user = await dbContext.Users
            .FirstOrDefaultAsync(x => (x.UserName == request.UserNameOrEmail
                                       || x.Email == request.UserNameOrEmail)
                                      && x.PasswordHash == HashAlgorithms
                                          .ConvertToHash(request.Password));
        if (user is null)
            return Result<Tuple<string, bool>>.Failure(
                Error.BadRequest(localizer.Get(TextKeys.Errors.InvalidCredentials)));

        await dbContext.SaveChangesAsync();
        return Result<Tuple<string, bool>>.Success(Tuple.Create(await service.GenerateTokenAsync(user), true));
    }

    public async Task<BaseResult> RegisterAsync(RegisterRequest request)
    {
        bool conflict = await dbContext.Users.AnyAsync(u =>
            u.UserName == request.UserName ||
            u.Email == request.Email ||
            u.PhoneNumber == request.PhoneNumber);

        if (conflict)
            return BaseResult.Failure(Error.Conflict(localizer.Get(TextKeys.Errors.UserAlreadyExists)));

        string roleName = DefaultRoles.User;

        var role = await dbContext.Roles
            .FirstOrDefaultAsync(r => r.Name == roleName);

        if (role is null)
            return BaseResult.Failure(Error.NotFound(localizer.Get(TextKeys.Errors.RoleNotFound)));

        var user = request.ToEntity();

        if (!IsValidDateOfBirth(request.DateOfBirth))
            return BaseResult.Failure(Error.BadRequest(localizer.Get(TextKeys.Errors.InvalidBirthDate)));

        await dbContext.Users.AddAsync(user);

        await dbContext.UserRoles.AddAsync(new UserRole
        {
            User = user,
            RoleId = role.Id
        });

        await dbContext.SaveChangesAsync();

        return BaseResult.Success();
    }

    public async Task<BaseResult> DeleteAccountAsync(int userId)
    {
        User? user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null) return BaseResult.Failure(ErrorFactory.NotFound(localizer));

        user.ToDelete();
        await dbContext.SaveChangesAsync();
        return BaseResult.Success();
    }

    public async Task<BaseResult> ChangePasswordAsync(int userId, ChangePasswordRequest request)
    {
        User? user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null) return BaseResult.Failure(ErrorFactory.NotFound(localizer));

        bool checkPassword = user.PasswordHash == HashAlgorithms.ConvertToHash(request.OldPassword);
        if (!checkPassword)
            return BaseResult.Failure(Error.BadRequest(localizer.Get(TextKeys.Errors.PasswordIncorrect)));

        if (!request.NewPassword.Equals(request.ConfirmPassword))
            return BaseResult.Failure(Error.BadRequest(localizer.Get(TextKeys.Errors.PasswordsMismatch)));

        user.PasswordHash = HashAlgorithms.ConvertToHash(request.NewPassword);

        await dbContext.SaveChangesAsync();
        return BaseResult.Success();
    }

    private static bool IsValidDateOfBirth(DateTimeOffset dateOfBirth)
    {
        return dateOfBirth <= DateTime.UtcNow && dateOfBirth >= DateTime.UtcNow.AddYears(-150);
    }
}
