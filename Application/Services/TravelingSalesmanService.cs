using Microsoft.EntityFrameworkCore;
using Application.ServiceContracts;
using Microsoft.Extensions.DependencyInjection;
using Application.Database;
using Application.Entities;
using Application.Models;

namespace Application.Services;

public class TravelingSalesmanService : ITravelingSalesmanService
{
    private static int _workload = 0;
    private readonly object _lock = new();
    private readonly ApplicationDbContext _db;
    private readonly IServiceProvider _serviceProvider;

    public int Workload
    {
        get
        {
            lock (_lock)
            {
                return _workload;
            }
        }

        private set
        {
            lock (_lock)
            {
                _workload = value;
            }
        }
    }

    public TravelingSalesmanService(ApplicationDbContext db, IServiceProvider serviceProvider)
    {
        _db = db;
        _serviceProvider = serviceProvider;
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
        _ = SolveAsync(inputData);

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

    public async Task<List<TravelingSalesmanInputData>> GetInputHistoryAsync()
        => await _db.InputData.AsNoTracking().ToListAsync();

    private async Task SolveAsync(TravelingSalesmanInputData inputData)
    {
        List<Point> points = inputData.Points;
        int n = points.Count;
        bool[] visited = new bool[n];
        List<Point> bestPath = [];
        List<Point> currentPath = [];
        double bestCost = double.MaxValue;
        int totalPathesAmount = Enumerable.Range(1, n).Aggregate((a, b) => a * b);
        int currentPathesAmount = 0;
        int lastReportedProgress = -1;

        double[,] distances = new double[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                distances[i, j] = points[i].DistanceTo(points[j]);
            }
        }

        using var scope = _serviceProvider.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var progress = await db.Progresses.FindAsync(inputData.Id);

        async Task DFS(int currentIndex, double currentCost)
        {
            if (currentCost >= bestCost)
            {
                return;
            }

            if (currentPath.Count == n)
            {
                currentCost += distances[currentIndex, 0];
                if (currentCost < bestCost)
                {
                    bestCost = currentCost;
                    bestPath = new List<Point>(currentPath);
                }
                return;
            }

            for (int i = 0; i < n; i++)
            {
                if (!visited[i])
                {
                    visited[i] = true;
                    currentPath.Add(points[i]);

                    await DFS(i, currentCost + (currentPath.Count > 1 ? distances[currentIndex, i] : 0));

                    currentPath.RemoveAt(currentPath.Count - 1);
                    visited[i] = false;
                }

                currentPathesAmount++;
                int percentage = (int)(currentPathesAmount * 100.0d / totalPathesAmount);
                if (percentage > lastReportedProgress + 3)
                {
                    lastReportedProgress = percentage;
                    progress.Progress = percentage;
                    db.Progresses.Update(progress);
                    await db.SaveChangesAsync();
                }
            }
        }

        visited[0] = true;
        currentPath.Add(points[0]);
        await DFS(0, 0);

        TravelingSalesmanResult result = new()
        {
            Id = inputData.Id,
            Path = bestPath,
            TotalDistance = bestCost,
            ComputedAt = DateTime.Now
        };

        await db.Results.AddAsync(result);
        progress.Progress = 100;
        db.Progresses.Update(progress);
        await db.SaveChangesAsync();
        Workload -= n;
    }
}
