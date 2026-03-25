using Application.Contracts.Localization;
using Application.Contracts.Services;
using Application.DTO_s;
using Application.Localization;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;

namespace Application.Extensions.Mappers;

public static class CarDocumentMap
{
    public static CarDocumentReadInfo ToRead(this CarDocument document, IFileService fileService, ITextLocalizer localizer)
    {
        var fileUrl = string.IsNullOrWhiteSpace(document.FilePath)
            ? null
            : fileService.GetFileUrl(document.FilePath, MediaFolders.Docs);

        return new CarDocumentReadInfo(
            Id: document.Id,
            CarId: document.CarId,
            DocumentType: document.DocumentType,
            DocumentTypeText: document.DocumentType.ToLocalizedString(localizer),
            FilePath: fileUrl!,
            VerificationStatus: document.VerificationStatus,
            VerificationStatusText: document.VerificationStatus.ToLocalizedString(localizer),
            CreatedAt: document.CreatedAt
        );
    }

    public static async Task<CarDocument> ToEntity(this CarDocumentCreateInfo createInfo, IFileService fileService)
    {
        var path = await fileService.CreateFile(
            createInfo.File,
            MediaFolders.Docs);

        return new CarDocument
        {
            CarId = createInfo.CarId,
            DocumentType = createInfo.DocumentType,
            FilePath = path,
            VerificationStatus = DocumentVerificationStatus.Pending
        };
    }

    public static CarDocument ToEntity(
        this CarDocument entity,
        CarDocumentUpdateInfo updateInfo, int adminId)
    {
        entity.VerificationStatus = updateInfo.VerificationStatus;
        entity.VerifiedByAdminId = adminId;
        entity.VerifiedAt = DateTimeOffset.UtcNow;
        entity.Version++;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        return entity;
    }
}
