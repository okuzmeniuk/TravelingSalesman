using Microsoft.AspNetCore.Mvc;
using Domain.ServiceContracts;
using Domain.Models;

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
		public async Task<IActionResult> StartSolving(List<Point> points)
		{
			return Ok(await _travelingSalesmanService.StartSolveAsync(points));
		}
	}
}
