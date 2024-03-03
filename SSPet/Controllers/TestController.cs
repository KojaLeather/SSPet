using Microsoft.AspNetCore.Mvc;
using SSPet.Services;
using System.IO;

[ApiController]
[Route("api/[controller]")]
public class StreamController : ControllerBase
{
    private readonly IRtmpService _rtmpService;

    public StreamController(IRtmpService rtmpService)
    {
        _rtmpService = rtmpService;
    }

    [HttpPost("start")]
    public IActionResult StartStreaming()
    {
        _rtmpService.Start();
        return Ok("Streaming started.");
    }

    [HttpPost("stop")]
    public IActionResult StopStreaming()
    {
        _rtmpService.Stop();
        return Ok("Streaming stopped.");
    }
}