using Application.DTO_s;
using Domain.Entities;
using Domain.Enums;

namespace Application.Extensions.Mappers;

public static class PaymentMap
{
    public static PaymentReadInfo ToRead(this Payment payment)
    {
        return new PaymentReadInfo(
            Id: payment.Id,
            BookingId: payment.BookingId,
            Amount: payment.Amount,
            PaymentMethod: payment.PaymentMethod,
            PaymentStatus: payment.Status,
            CreatedAt: payment.CreatedAt
        );
    }

    public static Payment ToEntity(this PaymentCreateInfo createInfo)
    {
        return new Payment()
        {
            BookingId = createInfo.BookingId,
            Amount = createInfo.Amount,
            PaymentMethod = PaymentMethod.Offline,
            Status = PaymentStatus.PendingAgreement
        };
    }
    
    public static Payment ToEntity(
        this Payment entity,
        PaymentStatusUpdateInfo updateInfo)
    {
        entity.Status = updateInfo.PaymentStatus;

        entity.Version++;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        return entity;
    }
}