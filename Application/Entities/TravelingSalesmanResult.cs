using Application.Models;

namespace Application.Entities;

public class TravelingSalesmanResult
{
    public Guid Id { get; set; }
    public List<Point> Path { get; set; } = [];
    public double TotalDistance { get; set; }
    public DateTime ComputedAt { get; set; }
}
