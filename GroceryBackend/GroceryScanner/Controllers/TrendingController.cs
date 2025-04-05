using GroceryScanner.Business.Service.Trending;
using Microsoft.AspNetCore.Mvc;

namespace GroceryScanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrendingController : ControllerBase
    {
        private readonly ITrendingService _trendingService;

        public TrendingController(ITrendingService trendingService)
        {
            _trendingService = trendingService;
        }

        [HttpPost("log")]
        public async Task<IActionResult> LogSearch([FromBody] string term)
        {
            await _trendingService.LogSearchAsync(term);
            return Ok("Logged");
        }

        [HttpGet("top-k")]
        public async Task<IActionResult> GetTopK([FromQuery] int k = 10)
        {
            var topItems = await _trendingService.GetTopKTrendingAsync(k);
            return Ok(topItems);
        }
    }

}
