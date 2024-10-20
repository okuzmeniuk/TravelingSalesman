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
		public async Task<IActionResult> StartSolving(List<Point> points)
		{
			return Ok(await _travelingSalesmanService.StartSolveAsync(points));
		}

		[HttpGet("progress/{id:guid}")]
		public async Task<IActionResult> GetProgress(Guid id)
		{
			try
			{
				return Ok(await _travelingSalesmanService.GetProgressAsync(id));
			}
			catch (ArgumentException ex)
			{
				return StatusCode(StatusCodes.Status404NotFound, ex.Message);
			}
		}

		[HttpGet("result/{id:guid}")]
		public async Task<IActionResult> GetResult(Guid id)
		{
			try
			{
				return Ok(await _travelingSalesmanService.GetResultAsync(id));
			}
			catch (ArgumentException ex)
			{
				return StatusCode(StatusCodes.Status404NotFound, ex.Message);
			}
		}
	}
}
