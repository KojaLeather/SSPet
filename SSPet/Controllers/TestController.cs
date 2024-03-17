using Microsoft.AspNetCore.Cors;
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
    public IActionResult StartStreaming(int id)
    {
        string result =_rtmpService.Start(id);
        return Ok(result);
    }

    [HttpPost("stop")]
    public IActionResult StopStreaming(int id)
    {
        string result = _rtmpService.Stop(id);
        return Ok(result);
    }
}