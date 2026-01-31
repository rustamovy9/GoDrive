using Application.DTO_s;
using Domain.Entities;

namespace Application.Extensions.Mappers;

public static class LocationMap
{
    public static LocationReadInfo ToRead(this Location location)
    {
        return new LocationReadInfo(
            Id: location.Id,
            Country: location.Country,
            City: location.City,
            Latitude: location.Latitude,
            Longitude: location.Longitude
        );
    }
    
    public static Location ToEntity(this LocationCreateInfo createInfo)
    {
        return new Location
        {
            Country = createInfo.Country,
            City = createInfo.City,
            Latitude = createInfo.Latitude,
            Longitude = createInfo.Longitude
        };
    }
    
    public static Location ToEntity(
        this Location entity,
        LocationUpdateInfo updateInfo)
    {
        if (updateInfo.Country is not null)
            entity.Country = updateInfo.Country;

        if (updateInfo.City is not null)
            entity.City = updateInfo.City;

        if (updateInfo.Latitude.HasValue)
            entity.Latitude = updateInfo.Latitude.Value;

        if (updateInfo.Longitude.HasValue)
            entity.Longitude = updateInfo.Longitude.Value;

        entity.Version++;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        return entity;
    }
}