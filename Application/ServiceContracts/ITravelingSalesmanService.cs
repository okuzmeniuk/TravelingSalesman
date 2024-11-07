using Application.Entities;
using Application.Models;

namespace Application.ServiceContracts;

public interface ITravelingSalesmanService
{
    int Workload { get; }

    Task<TravelingSalesmanInputData> StartSolveAsync(List<Point> points);
    Task<ProblemSolvingProgress> GetProgressAsync(Guid id);
    Task<TravelingSalesmanResult> GetResultAsync(Guid id);
    Task<List<TravelingSalesmanInputData>> GetInputHistoryAsync();
}
