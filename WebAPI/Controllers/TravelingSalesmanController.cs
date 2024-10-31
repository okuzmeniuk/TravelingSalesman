using Application.Models;
using Application.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/traveling-salesman")]
[ApiController]
public class TravelingSalesmanController : ControllerBase
{
    private readonly ITravelingSalesmanService _travelingSalesmanService;

    public TravelingSalesmanController(ITravelingSalesmanService travelingSalesmanService)
    {
        _travelingSalesmanService = travelingSalesmanService;
    }

    [HttpGet("workload")]
    public IActionResult GetWorkload() => Ok(_travelingSalesmanService.Workload);

    [HttpPost("solve")]
    public async Task<IActionResult> StartSolving(List<Point> points)
        => points.Count > 30
        ? BadRequest("Amount of points shouldn't exceed 30")
        : Ok(await _travelingSalesmanService.StartSolveAsync(points));
}
