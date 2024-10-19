using WebAPI.Entities;
using WebAPI.ServiceContracts;

namespace WebAPI.Services
{
	public class TravelingSalesmanService : ITravelingSalesmanService
	{
		private int _workload = 0;
		private readonly object _lock = new();

		public int Workload
		{
			get => _workload;
			private set
			{
				lock (_lock)
				{
					_workload = value;
				}
			}
		}

		public Task<TravelingSalesmanResult> SolveProblemAsync(TravelingSalesmanInputData input)
		{
			Workload += input.Points.Count;
			var result = new TravelingSalesmanResult { ComputedAt = DateTime.UtcNow, Path = [1, 2, 3], Id = input.Id };
			Workload -= input.Points.Count;
			return Task.FromResult(result);
		}
	}
}
