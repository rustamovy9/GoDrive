namespace Application.DTO_s;

public interface IBaseRentalCompanyInfo
{
    public string Name { get; init; }
    public string? ContactInfo { get; init; }
}

public readonly record struct RentalCompanyReadInfo(
    string Name,
    string? ContactInfo,
    int Id) : IBaseRentalCompanyInfo;

public readonly record struct RentalCompanyUpdateInfo(
    string Name,
    string? ContactInfo) : IBaseRentalCompanyInfo;

public readonly record struct RentalCompanyCreateInfo(
    string Name,
    string? ContactInfo) : IBaseRentalCompanyInfo;
