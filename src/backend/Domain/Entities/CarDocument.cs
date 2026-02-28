using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class CarDocument : BaseEntity
{
    public int CarId { get; set; }
    public CarDocumentType DocumentType { get; set; }
    public string FilePath { get; set; } = null!;
    
    public int? VerifiedByAdminId { get; set; }
    
    public DateTimeOffset VerifiedAt { get; set; }

    public DocumentVerificationStatus VerificationStatus { get; set; }
    
    public User? VerifiedByAdmin { get; set; }
    public Car Car { get; set; } = null!;
}