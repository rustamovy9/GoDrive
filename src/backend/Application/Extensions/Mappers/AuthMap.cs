using Application.DTO_s;
using Application.Extensions.Algorithms;
using Domain.Entities;

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
            Email = request.Email,
            PasswordHash = HashAlgorithms.ConvertToHash(request.Password),
        };
    }
}