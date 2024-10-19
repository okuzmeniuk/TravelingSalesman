namespace WebAPI.Entities
{
	public class TravelingSalesmanResult
	{
		public Guid Id { get; set; }
		public List<int> Path { get; set; } = [];
		public DateTime ComputedAt { get; set; }
	}
}
