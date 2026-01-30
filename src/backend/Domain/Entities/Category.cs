using Domain.Common;

namespace Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;

    public ICollection<Car> Cars { get; set; } = [];
}