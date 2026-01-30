using Domain.Common;

namespace Domain.Entities;

public class RentalCompany : BaseEntity
{
    public string Name { get; set; } = null!;
    public string? ContactInfo { get; set; }
    public int OwnerId { get; set; }

    public User Owner { get; set; } = null!; 
    public ICollection<Car> Cars { get; set; } = [];
}
