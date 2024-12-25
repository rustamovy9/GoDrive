using Application.DTO_s;
using Domain.Entities;
using Infrastructure.Extensions.Algorithms;

namespace Application.Extensions.Mappers;

public static class AuthMap
{
    public static User ToEntity(this RegisterRequest request)
    {
        return new()
        {
            UserName = request.UserName,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            PhoneNumber = request.PhoneNumber,
            Email = request.EmailAddress,
            PasswordHash = HashAlgorithms.ConvertToHash(request.Password),
        };
    }

    public static User ToDelete(this User user)
    {
        user.IsDeleted = true;
        user.IsActive = false;
        user.DeletedAt = DateTimeOffset.UtcNow;
        user.Version++;
        return user;
    }
}