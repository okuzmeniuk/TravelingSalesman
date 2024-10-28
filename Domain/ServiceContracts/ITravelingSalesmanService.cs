using Domain.Entities;
using Domain.Entities;
using Domain.Models;

namespace Domain.ServiceContracts
{
    public interface ITravelingSalesmanService
	{
		int Workload { get; }

		Task<TravelingSalesmanInputData> StartSolveAsync(List<Point> points);
		Task<ProblemSolvingProgress> GetProgressAsync(Guid id);
		Task<TravelingSalesmanResult> GetResultAsync(Guid id);
	}
}
