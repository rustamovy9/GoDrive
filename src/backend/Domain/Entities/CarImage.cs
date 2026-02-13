using Domain.Common;
using Domain.Constants;

namespace Domain.Entities;

public class CarImage : BaseEntity
{
    public int CarId { get; set; }
    public string ImagePath { get; set; } = FileData.Default;
    public bool IsMain { get; set; }
    public Car Car { get; set; } = null!;
}