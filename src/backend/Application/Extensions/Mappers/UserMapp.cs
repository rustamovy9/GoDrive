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
            user.UserName,
            user.FirstName,
            user.LastName,
            user.DateOfBirth,
            user.Email,
            user.PhoneNumber,
            user.Address,
            user.DriverLicense,
            user.AvatarPath,
            user.Id
        );
    }
    

    public static async Task<User> ToEntity(this User entity, UserUpdateInfo updateInfo,IFileService fileService)
    {
        if (updateInfo.File is not null)
        { 
            fileService.DeleteFile(entity.AvatarPath, MediaFolders.Images);
            
            entity.AvatarPath = await fileService.CreateFile(updateInfo.File, MediaFolders.Images);
        }
        entity.UserName = updateInfo.UserName;
        entity.FirstName = updateInfo.FirstName;
        entity.LastName = updateInfo.LastName;
        entity.DateOfBirth = updateInfo.DateOfBirth;
        entity.Email = updateInfo.Email;
        entity.PhoneNumber = updateInfo.PhoneNumber ;
        entity.DriverLicense = updateInfo.DriverLicense ;
        entity.Version++;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        return entity;
    }
}