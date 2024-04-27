using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSPet.Services.Interfaces;

namespace SSPet.Controllers
{
    public class StreamRequest
    {
        public int Id { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class StreamController : ControllerBase
    {
        private string? result;
        private readonly IRtmpService _rtmpService;

        public StreamController(IRtmpService rtmpService)
        {
            _rtmpService = rtmpService;
        }

        [HttpGet("start")]
        public async Task<IActionResult> StartAsync()
        {
            result = await _rtmpService.StartAsync();
            return Ok(new { result });
        }

        [HttpGet("stop")]
        public async Task<IActionResult> StopAsync()
        {
            result = await _rtmpService.StopAsync();
            return Ok(new { result });
        }
    }
}
