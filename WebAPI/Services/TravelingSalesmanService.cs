using WebAPI.Models;
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

		public Task<List<int>> SolveProblemAsync(List<Point> points)
		{
			Workload += points.Count;
			var result = new List<int> { 1, 2, 3 };
			Workload -= points.Count;
			return Task.FromResult(result);
		}
	}
}
