using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class CarPrice : BaseEntity
{
    public int CarId { get; set; }
    public decimal PricePerDay { get; set; }
    public Currency Currency { get; set; }

    public Car Car { get; set; } = null!;
}