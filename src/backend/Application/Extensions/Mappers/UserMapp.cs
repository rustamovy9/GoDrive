using Application.Contracts.Services;
using Application.DTO_s;
using Domain.Constants;
using Domain.Entities;


namespace Application.Extensions.Mappers;

public static class UserMapper
{
    public static UserReadInfo ToRead(this User user)
    {
        return new UserReadInfo(
            user.Id,
            user.UserName,
            user.FirstName,
            user.LastName,
            user.DateOfBirth,
            user.Email,
            user.PhoneNumber,
            user.Address,
            user.DriverLicense,
            user.AvatarPath,
            user.CreatedAt
        );
    }
    

    public static async Task<User> ToEntity(this User entity, UserUpdateInfo updateInfo,IFileService fileService)
    {
        if (updateInfo.FirstName is not null)
            entity.FirstName = updateInfo.FirstName;

        if (updateInfo.LastName is not null)
            entity.LastName = updateInfo.LastName;

        if (updateInfo.PhoneNumber is not null)
            entity.PhoneNumber = updateInfo.PhoneNumber;

        if (updateInfo.Address is not null)
            entity.Address = updateInfo.Address;

        if (updateInfo.DriverLicense is not null)
            entity.DriverLicense = updateInfo.DriverLicense;

        if (updateInfo.AvatarPath is not null)
        {
            if (!string.IsNullOrWhiteSpace(entity.AvatarPath))
                fileService.DeleteFile(entity.AvatarPath, MediaFolders.Images);

            entity.AvatarPath = await fileService.CreateFile(
                updateInfo.AvatarPath,
                MediaFolders.Images);
        }

        entity.Version++;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        return entity;
    }
}