using Microsoft.AspNetCore.Mvc;
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
	}
}
