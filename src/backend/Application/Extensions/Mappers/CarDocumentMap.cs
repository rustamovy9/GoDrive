using Application.Contracts.Services;
using Application.DTO_s;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;

namespace Application.Extensions.Mappers;

public static class CarDocumentMap
{
    public static CarDocumentReadInfo ToRead(this CarDocument document)
    {
        return new CarDocumentReadInfo(
            Id: document.Id,
            CarId: document.CarId,
            DocumentType: document.DocumentType,
            FilePath: document.FilePath,
            VerificationStatus: document.VerificationStatus,
            AiConfidenceScore: document.AiConfidenceScore,
            CreatedAt: document.CreatedAt
        );
    }

    public static async Task<CarDocument> ToEntity(this CarDocumentCreateInfo createInfo, IFileService fileService)
    {
        var path = await fileService.CreateFile(
            createInfo.File,
            MediaFolders.Documents);
        
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
        CarDocumentUpdateInfo updateInfo,int adminId)
    {
        entity.VerificationStatus = updateInfo.VerificationStatus;
        entity.VerifiedByAdminId = adminId;
        entity.VerifiedAt = DateTimeOffset.UtcNow;
        entity.Version++;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        return entity;
    }
}