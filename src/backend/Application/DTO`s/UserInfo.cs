using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.DTO_s;

public sealed record UserReadInfo(
    int Id,
    string UserName,
    string FirstName,
    string LastName,
    DateTimeOffset DateOfBirth,
    string Email,
    string? PhoneNumber,
    string? Address,
    string? DriverLicense,
    string AvatarPath,
    DateTimeOffset CreatedAt);

public sealed record UserUpdateInfo(
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? Address,
    string? DriverLicense,
    IFormFile? File);
