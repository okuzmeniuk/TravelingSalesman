using WebAPI.Models;

namespace WebAPI.ServiceContracts
{
	public interface ITravelingSalesmanService
	{
		int Workload { get; }

		Task<List<int>> SolveProblemAsync(List<Point> points);
	}
}
