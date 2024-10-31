using Domain.Models;
using Domain.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace LoadBalancer.Controllers
{
    [Route("api/traveling-salesman")]
    [ApiController]
    public class TravelingSalesmanController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ITravelingSalesmanService _travelingSalesmanService;

        public TravelingSalesmanController(IConfiguration configuration, ITravelingSalesmanService travelingSalesmanService)
        {
            HttpClientHandler clientHandler = new()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            _httpClient = new HttpClient(clientHandler);

            _configuration = configuration;
            _travelingSalesmanService = travelingSalesmanService;
        }

        [HttpPost("solve")]
        public async Task<IActionResult> StartSolving(List<Point> points)
        {
            if (points.Count > 30)
            {
                return BadRequest("Amount of points shouldn't exceed 30");
            }

            var serverUrls = _configuration.GetSection("Servers")
                                           .AsEnumerable()
                                           .Where(pair => !string.IsNullOrEmpty(pair.Value))
                                           .Select(pair => new Uri(pair.Value));

            var workloadPerServerUrl = new Dictionary<Uri, int>();

            foreach (var serverUrl in serverUrls)
            {
                var request = new HttpRequestMessage(HttpMethod.Get, new Uri(serverUrl, "api/traveling-salesman/workload/"));
                var response = await _httpClient.SendAsync(request);
                var workload = int.Parse(await response.Content.ReadAsStringAsync());
                workloadPerServerUrl.Add(serverUrl, workload);
            }

            var serverWithMinWorkload = workloadPerServerUrl.MinBy(p => p.Value).Key;
            var startSolveRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(serverWithMinWorkload, "api/traveling-salesman/solve/"))
            {
                Content = JsonContent.Create(points),
            };

            var startSolveResponse = await _httpClient.SendAsync(startSolveRequest);
            return Ok(await startSolveResponse.Content.ReadAsStringAsync());
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
