using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.ServiceContracts;

namespace WebAPI.Controllers
{
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
		public IActionResult GetWorkload()
		{
			return Ok(_travelingSalesmanService.Workload);
		}

		[HttpPost("solve")]
		public async Task<IActionResult> PostSolveAsync(List<Point> points)
		{
			var result = await _travelingSalesmanService.SolveProblemAsync(points);
			return Ok(result);
		}
	}
}
