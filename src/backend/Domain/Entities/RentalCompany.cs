using Domain.Common;

namespace Domain.Entities;

public sealed class RentalCompany : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? ContactInfo { get; set; } 
    public List<Car> Cars { get; set; } = [];
}
