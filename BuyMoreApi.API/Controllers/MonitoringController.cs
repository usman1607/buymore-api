using BuyMoreApi.Application.Monitoring;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuyMoreApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous] // Metrics are unauthenticated so tooling can read them without a token.
    public class MonitoringController : ControllerBase
    {
        private readonly IMetricsService _metricsService;

        public MonitoringController(IMetricsService metricsService)
        {
            _metricsService = metricsService;
        }

        [HttpGet("metrics")]
        public IActionResult GetMetrics()
        {
            var snapshot = _metricsService.GetSnapshot();
            return Ok(snapshot);
        }
    }
}
