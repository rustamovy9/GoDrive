using Microsoft.AspNetCore.Http;

namespace Application.DTO_s;

public interface IBaseCarInfo
{
    public string Brand { get; init; }
    public string Model { get; init; }
    public int Year { get; init; }
    public string? Category { get; init; }
    public string RegistrationNumber { get; init; }
    public string? Location { get; init; }

}

public readonly record struct CarReadInfo(

    string Brand,
    string Model,
    int Year,
    string? Category,
    string RegistrationNumber,
    string? Location,
    int Id):IBaseCarInfo;

public readonly record struct CarUpdateInfo(
    string Brand,
    string Model,
    int Year,
    string? Category,
    string RegistrationNumber,
    string? Location
    ) : IBaseCarInfo;

public readonly record struct CarCreateInfo(
    string Name,
    string Code) : IBaseSpecializationInfo;
    
    
    