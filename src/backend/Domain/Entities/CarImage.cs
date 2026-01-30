using Domain.Common;
using Domain.Constants;

namespace Domain.Entities;

public class CarImage : BaseEntity
{
    public int CarId { get; set; }
    public string ImagePath { get; set; } = FileData.Default;

    public Car Car { get; set; } = null!;
}