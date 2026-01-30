namespace Application.DTO_s;

public sealed record CarImageReadInfo(
    int Id,
    int CarId,
    string ImagePath);
    
public sealed record CarImageCreateInfo(
    int CarId,
    string ImagePath);