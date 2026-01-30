using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Payment : BaseEntity
{
    public int BookingId { get; set; }
    public decimal Amount { get; set; }

    public PaymentMethod PaymentMethod { get; set; }
    public PaymentStatus Status { get; set; }

    public Booking Booking { get; set; } = null!;
}