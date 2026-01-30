using Application.DTO_s;
using Domain.Entities;

namespace Application.Extensions.Mappers;

public static class RentalCompanyMapper
{
    public static RentalCompanyReadInfo ToRead(this RentalCompany rentalCompany)
    {
        return new RentalCompanyReadInfo(
            Id: rentalCompany.Id,
            Name: rentalCompany.Name,
            ContactInfo: rentalCompany.ContactInfo,
            OwnerId: rentalCompany.OwnerId,
            CreatedAt: rentalCompany.CreatedAt
        );
    }

    public static RentalCompany ToEntity(this RentalCompanyCreateInfo createInfo,int ownerId)
    {
        return new RentalCompany
        {
            Name = createInfo.Name,
            ContactInfo = createInfo.ContactInfo,
            OwnerId = ownerId
        };
    }

    public static RentalCompany ToEntity(this RentalCompany entity, RentalCompanyUpdateInfo updateInfo)
    {
        if (updateInfo.Name is not null)
            entity.Name = updateInfo.Name;

        if (updateInfo.ContactInfo is not null)
            entity.ContactInfo = updateInfo.ContactInfo;

        entity.Version++;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        return entity;
    }
}