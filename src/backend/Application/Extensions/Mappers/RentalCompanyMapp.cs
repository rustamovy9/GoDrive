using Application.DTO_s;
using Domain.Entities;

namespace Application.Extensions.Mappers;

public static class RentalCompanyMapper
{
    public static RentalCompanyReadInfo ToRead(this RentalCompany rentalCompany)
    {
        return new RentalCompanyReadInfo(
            Name: rentalCompany.Name,
            ContactInfo: rentalCompany.ContactInfo,
            Id: rentalCompany.Id
        );
    }

    public static RentalCompany ToEntity(this RentalCompanyCreateInfo createInfo)
    {
        return new RentalCompany
        {
            Name = createInfo.Name,
            ContactInfo = createInfo.ContactInfo,
            Cars = new List<Car>() 
        };
    }

    public static RentalCompany ToEntity(this RentalCompany entity, RentalCompanyUpdateInfo updateInfo)
    {
        entity.Name = updateInfo.Name;
        entity.ContactInfo = updateInfo.ContactInfo;
        entity.Version++;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        return entity;
    }
}