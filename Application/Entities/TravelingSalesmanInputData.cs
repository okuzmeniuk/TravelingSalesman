using Application.Models;

namespace Application.Entities;

public class TravelingSalesmanInputData
{
    public Guid Id { get; set; }
    public List<Point> Points { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}
