using Domain.Models;

namespace Backend.Models;

public class RentalCompany : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? ContactInfo { get; set; } 
    public List<Car> Cars { get; set; } = new();
}
