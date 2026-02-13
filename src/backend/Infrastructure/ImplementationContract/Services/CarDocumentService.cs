using Application.Contracts.Repositories;
using Application.Contracts.Services;
using Application.DTO_s;
using Application.Extensions.Mappers;
using Application.Extensions.ResultPattern;
using Domain.Common;
using Domain.Constants;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ImplementationContract.Services;

public class CarDocumentService(
    ICarDocumentRepository repository,
    ICarRepository carRepository,
    IUserRepository userRepository,
    INotificationService notificationService,
    IFileService fileService) : ICarDocumentService
{
    public async Task<Result<IEnumerable<CarDocumentReadInfo>>> GetByCarIdAsync(int carId)
    {
        var res = repository.Find(x => x.CarId == carId);

        if (!res.IsSuccess)
            return Result<IEnumerable<CarDocumentReadInfo>>.Failure(res.Error);

        var data = await res.Value!
            .Select(x => x.ToRead())
            .ToListAsync();

        return Result<IEnumerable<CarDocumentReadInfo>>.Success(data);
    }

    public async Task<BaseResult> CreateAsync(CarDocumentCreateInfo createInfo)
    {
        var carRes = await carRepository.GetByIdAsync(createInfo.CarId);
        if (!carRes.IsSuccess || carRes.Value is null)
            return BaseResult.Failure(Error.NotFound("Car not found"));


        var document = await createInfo.ToEntity(fileService);

        var result = await repository.AddAsync(document);
        if (!result.IsSuccess)
            return BaseResult.Failure(result.Error);

        // 🔔 уведомление владельцу
        await notificationService.CreateAsync(
            new NotificationCreateInfo(
                carRes.Value.OwnerId,
                "Document submitted",
                "Your document has been submitted and is pending verification."
            )
        );

        // 🔔 уведомление админам
        var admins = await userRepository.GetAdminsAsync();


        if (admins.IsSuccess)
        {
            foreach (var admin in admins.Value!)
            {
                await notificationService.CreateAsync(
                    new NotificationCreateInfo(
                        admin.Id,
                        "New document uploaded",
                        $"A new car document requires verification."
                    )
                );
            }
        }

        return BaseResult.Success();
    }

    public async Task<BaseResult> UpdateStatusAsync(
        int id,
        int adminId,
        CarDocumentUpdateInfo updateInfo)
    {
        var documentRes = await repository.GetByIdAsync(id);

        if (!documentRes.IsSuccess || documentRes.Value is null)
            return BaseResult.Failure(Error.NotFound("Document not found"));

        var adminCheck = await userRepository.IsAdminAsync(adminId);
        if (!adminCheck.IsSuccess || !adminCheck.Value)
            return BaseResult.Failure(Error.Forbidden("Only admin can verify documents"));

        var document = documentRes.Value;

        if (document.VerificationStatus != DocumentVerificationStatus.Pending)
            return BaseResult.Failure(Error.BadRequest("Document already reviewed"));

        document.ToEntity(updateInfo, adminId);

        var updateResult = await repository.UpdateAsync(document);
        if (!updateResult.IsSuccess)
            return BaseResult.Failure(updateResult.Error);

        // 🔄 Update Car Status
        var carRes = await carRepository.GetByIdAsync(document.CarId);

        if (carRes.IsSuccess && carRes.Value is not null)
        {
            var car = carRes.Value;

            car.CarStatus =
                updateInfo.VerificationStatus == DocumentVerificationStatus.ApprovedByAdmin
                    ? CarStatus.Available
                    : CarStatus.Blocked;

            car.UpdatedAt = DateTimeOffset.UtcNow;
            car.Version++;

            await carRepository.UpdateAsync(car);

            // 🔔 Owner notification
            await notificationService.CreateAsync(
                new NotificationCreateInfo(
                    car.OwnerId,
                    "Document reviewed",
                    $"Your document has been {updateInfo.VerificationStatus.ToString().ToLowerInvariant()} by admin."
                ));
        }

        return BaseResult.Success();
    }
    

    public async Task<BaseResult> DeleteAsync(
        int id,
        int currentUserId,
        bool isAdmin)
    {
        var documentRes = await repository.GetByIdAsync(id);

        if (!documentRes.IsSuccess || documentRes.Value is null)
            return BaseResult.Failure(Error.NotFound("Document not found"));

        var document = documentRes.Value;

        var carRes = await carRepository.GetByIdAsync(document.CarId);

        if (!carRes.IsSuccess || carRes.Value is null)
            return BaseResult.Failure(Error.NotFound("Car not found"));

        var car = carRes.Value;

        // 🔐 Access control
        if (!isAdmin && car.OwnerId != currentUserId)
            return BaseResult.Failure(Error.Forbidden());

        fileService.DeleteFile(document.FilePath, MediaFolders.Documents);

        var deleteResult = await repository.DeleteAsync(id);

        return deleteResult.IsSuccess
            ? BaseResult.Success()
            : BaseResult.Failure(deleteResult.Error);
    }
    
    public async Task<Result<(byte[] FileBytes, string FileName)>> DownloadAsync(
        int id,
        int currentUserId,
        bool isAdmin)
    {
        var documentRes = await repository.GetByIdAsync(id);

        if (!documentRes.IsSuccess || documentRes.Value is null)
            return Result<(byte[], string)>.Failure(Error.NotFound("Document not found"));

        var document = documentRes.Value;

        var carRes = await carRepository.GetByIdAsync(document.CarId);

        if (!carRes.IsSuccess || carRes.Value is null)
            return Result<(byte[], string)>.Failure(Error.NotFound("Car not found"));

        var car = carRes.Value;

        if (!isAdmin && car.OwnerId != currentUserId)
            return Result<(byte[], string)>.Failure(Error.Forbidden());

        if (!fileService.FileExists(document.FilePath))
            return Result<(byte[], string)>.Failure(Error.NotFound("File not found"));

        var file = await fileService.GetFileAsync(document.FilePath);

        return Result<(byte[], string)>.Success(file);
    }
}