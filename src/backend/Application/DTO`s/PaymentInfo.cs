using Domain.Enums;

namespace Application.DTO_s;

public sealed record PaymentReadInfo(
    int Id, 
    int BookingId,
    decimal Amount,
    PaymentMethod PaymentMethod,
    PaymentStatus PaymentStatus,
    DateTimeOffset CreatedAt);   
    
public sealed record PaymentStatusUpdateInfo(PaymentStatus PaymentStatus);