namespace Application.DTO_s.AI;

public class CarAiContext
{
    public int CarId { get; set; }
    public string Brand { get; set; } = "";
    public string Model { get; set; } = "";
    public decimal PricePerDay { get; set; }
    public double Rating { get; set; }
    public int Year { get; set; }
    public string City { get; set; } = "";
}