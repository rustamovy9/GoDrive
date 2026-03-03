using Application.Contracts.Services;
using Application.DTO_s;
using Domain.Constants;
using Domain.Entities;


namespace Application.Extensions.Mappers;

public static class UserMapper
{
    public static UserReadInfo ToRead(this User user,IFileService fileService)
    {
        var avatarUrl = string.IsNullOrWhiteSpace(user.AvatarPath)
            ? null
            :  fileService.GetFileUrl(user.AvatarPath, MediaFolders.Images);
        
        return new UserReadInfo(
            user.Id,
            user.UserName,
            user.FirstName,
            user.LastName,
            user.DateOfBirth,
            user.Email,
            user.PhoneNumber,
            user.Address,
            avatarUrl!,
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

        if (updateInfo.File is not null)
        {
            if (!string.IsNullOrWhiteSpace(entity.AvatarPath))
                await fileService.DeleteFile(entity.AvatarPath, MediaFolders.Images);

            entity.AvatarPath = await fileService.CreateFile(
                updateInfo.File,
                MediaFolders.Images);
        }

        entity.Version++;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        return entity;
    }
}