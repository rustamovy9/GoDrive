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
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            PasswordHash = HashAlgorithms.ConvertToHash(request.Password),
        };
    }
}