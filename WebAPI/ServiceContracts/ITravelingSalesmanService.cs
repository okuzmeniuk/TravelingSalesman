using WebAPI.Entities;

namespace WebAPI.ServiceContracts
{
	public interface ITravelingSalesmanService
	{
		int Workload { get; }

		Task<TravelingSalesmanResult> SolveProblemAsync(TravelingSalesmanInputData input);
	}
}
