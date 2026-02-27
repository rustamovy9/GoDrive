using Application.Contracts.AI;
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
    IFileService fileService,
    IAiDocumentService aiDocumentService) : ICarDocumentService
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

        // 1. сохраняем файл
        var document = await createInfo.ToEntity(fileService);

        // 2. AI verification
        var aiResult = await aiDocumentService.VerifyAsync(document.FilePath);

        Console.WriteLine("AI Success: " + aiResult.IsSuccess);

        if (aiResult.IsSuccess)
        {
            Console.WriteLine("AI Valid: " + aiResult.Value!.IsValid);
        }
        
        if (aiResult.IsSuccess)
        {
            document.AiConfidenceScore = aiResult.Value!.ConfidenceScore;
            document.AiExtractedDataJson = aiResult.Value.ExtractedJson;

            document.VerificationStatus = aiResult.Value.IsValid
                ? DocumentVerificationStatus.AutoApproved
                : DocumentVerificationStatus.AutoRejected;
        }
        else
        {
            document.VerificationStatus = DocumentVerificationStatus.Pending;
        }

        // 3. save document
        var result = await repository.AddAsync(document);

        if (!result.IsSuccess)
            return BaseResult.Failure(result.Error);

        await RecalculateCarStatus(carRes.Value.Id);

        // уведомление owner
        await notificationService.CreateAsync(
            new NotificationCreateInfo(
                carRes.Value.OwnerId,
                "Document uploaded",
                $"Document was automatically {document.VerificationStatus}"
            )
        );

        // уведомление admin только если rejected
        if (document.VerificationStatus == DocumentVerificationStatus.AutoRejected)
        {
            var admins = await userRepository.GetAdminsAsync();

            if (admins.IsSuccess)
            {
                foreach (var admin in admins.Value!)
                {
                    await notificationService.CreateAsync(
                        new NotificationCreateInfo(
                            admin.Id,
                            "Document requires review",
                            $"Document #{document.Id} was auto rejected by AI"
                        )
                    );
                }
            }
        }

        return BaseResult.Success();
    }

    public async Task<BaseResult> UpdateAsync(
        int id,
        CarDocumentCreateInfo updateInfo,
        int currentUserId)
    {
        var documentRes = await repository.GetByIdAsync(id);

        if (!documentRes.IsSuccess || documentRes.Value is null)
            return BaseResult.Failure(Error.NotFound("Document not found"));

        var document = documentRes.Value;

        var carRes = await carRepository.GetByIdAsync(document.CarId);

        if (!carRes.IsSuccess || carRes.Value is null)
            return BaseResult.Failure(Error.NotFound("Car not found"));

        var car = carRes.Value;

        if (car.OwnerId != currentUserId)
            return BaseResult.Failure(Error.Forbidden());

        // удалить старый файл
        await fileService.DeleteFile(document.FilePath, MediaFolders.Docs);

        // сохранить новый файл
        var newPath = await fileService.CreateFile(
            updateInfo.File,
            MediaFolders.Docs);

        document.FilePath = newPath;

        // 🔥 AI verification
        var aiRes = await aiDocumentService.VerifyAsync(newPath);

        if (aiRes.IsSuccess)
        {
            document.AiConfidenceScore = aiRes.Value!.ConfidenceScore;
            document.AiExtractedDataJson = aiRes.Value.ExtractedJson;

            document.VerificationStatus =
                aiRes.Value.IsValid
                    ? DocumentVerificationStatus.AutoApproved
                    : DocumentVerificationStatus.AutoRejected;
        }
        else
        {
            document.VerificationStatus = DocumentVerificationStatus.Pending;
        }

        // reset admin verification
        document.VerifiedAt = DateTimeOffset.UtcNow;
        document.VerifiedByAdminId = null;

        document.UpdatedAt = DateTimeOffset.UtcNow;
        document.Version++;

        var update = await repository.UpdateAsync(document);

        if (!update.IsSuccess)
            return BaseResult.Failure(update.Error);

        await RecalculateCarStatus(document.CarId);

        // 🔔 уведомление владельцу
        await notificationService.CreateAsync(
            new NotificationCreateInfo(
                car.OwnerId,
                "Document updated",
                $"Your document \"{document.DocumentType}\" has been updated and re-verified."
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
                        "Document updated",
                        $"Car #{car.Id} document \"{document.DocumentType}\" requires verification."
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


        var document = documentRes.Value;

        if (document.VerificationStatus != DocumentVerificationStatus.Pending)
            return BaseResult.Failure(Error.BadRequest("Document already reviewed"));

        document.ToEntity(updateInfo, adminId);

        var updateResult = await repository.UpdateAsync(document);
        if (!updateResult.IsSuccess)
            return BaseResult.Failure(updateResult.Error);

        await RecalculateCarStatus(document.CarId);
        // 🔄 Update Car Status
        var carRes = await carRepository.GetByIdAsync(document.CarId);

        if (carRes.IsSuccess && carRes.Value is not null)
        {
            // 🔔 Owner notification
            await notificationService.CreateAsync(
                new NotificationCreateInfo(
                    carRes.Value.OwnerId,
                    "Document reviewed",
                    $"Your document has been {updateInfo.VerificationStatus}."
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

        await fileService.DeleteFile(document.FilePath, MediaFolders.Docs);

        var deleteResult = await repository.DeleteAsync(id);

        if (!deleteResult.IsSuccess)
            return BaseResult.Failure(deleteResult.Error);

        await RecalculateCarStatus(car.Id);

        return BaseResult.Success();
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

        // 🔥 вместо FileExists + GetFileAsync
        var file = await fileService.DownloadAsync(
            document.FilePath,
            MediaFolders.Docs
        );

        return Result<(byte[], string)>.Success(file);
    }

    private async Task RecalculateCarStatus(int carId)
    {
        var documentsRes = repository.Find(x => x.CarId == carId && !x.IsDeleted);

        if (!documentsRes.IsSuccess)
            return;

        var documents = await documentsRes.Value!.ToListAsync();

        var carRes = await carRepository.GetByIdAsync(carId);

        if (!carRes.IsSuccess || carRes.Value is null)
            return;

        var car = carRes.Value;

        if (!documents.Any())
        {
            car.CarStatus = CarStatus.Blocked;
        }
        else if (documents.Any(d =>
                     d.VerificationStatus == DocumentVerificationStatus.Pending))
        {
            car.CarStatus = CarStatus.PendingApproval;
        }
        else if (documents.Any(d =>
                     d.VerificationStatus == DocumentVerificationStatus.AutoRejected ||
                     d.VerificationStatus == DocumentVerificationStatus.RejectedByAdmin))
        {
            car.CarStatus = CarStatus.Blocked;
        }
        else if (documents.All(d =>
                     d.VerificationStatus == DocumentVerificationStatus.AutoApproved ||
                     d.VerificationStatus == DocumentVerificationStatus.ApprovedByAdmin))
        {
            car.CarStatus = CarStatus.Available;
        }

        car.UpdatedAt = DateTimeOffset.UtcNow;
        car.Version++;

        await carRepository.UpdateAsync(car);
    }
}