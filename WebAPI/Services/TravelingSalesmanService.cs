using Microsoft.EntityFrameworkCore;
using WebAPI.Database;
using WebAPI.Entities;
using WebAPI.Models;
using WebAPI.ServiceContracts;

namespace WebAPI.Services
{
	public class TravelingSalesmanService : ITravelingSalesmanService
	{
		private int _workload = 0;
		private readonly object _lock = new();
		private readonly ApplicationDbContext _db;

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

		public TravelingSalesmanService(ApplicationDbContext db)
		{
			_db = db;
		}

		public async Task<TravelingSalesmanInputData> StartSolveAsync(List<Point> points)
		{
			Workload += points.Count;
			TravelingSalesmanInputData inputData = new()
			{
				Id = Guid.NewGuid(),
				Points = points,
				CreatedAt = DateTime.UtcNow
			};

			_db.InputData.Add(inputData);
			_db.Progresses.Add(new ProblemSolvingProgress
			{
				Id = inputData.Id,
				Progress = 0
			});

			await _db.SaveChangesAsync();
			SolveAsync(inputData);

			return inputData;
		}

		public async Task<ProblemSolvingProgress> GetProgressAsync(Guid id)
		{
			ProblemSolvingProgress progress = await _db.Progresses.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id)
				?? throw new ArgumentException("Invalid ID");

			return progress;
		}

		public async Task<TravelingSalesmanResult> GetResultAsync(Guid id)
		{
			TravelingSalesmanResult result = await _db.Results.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id)
				?? throw new ArgumentException("Process didn't finish or invalid ID");

			return result;
		}

		private void SolveAsync(TravelingSalesmanInputData inputData)
		{
			// Solve the problem
			throw new NotImplementedException();
		}
	}
}
