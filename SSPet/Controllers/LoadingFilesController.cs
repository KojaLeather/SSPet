using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSPet.Models;
using SSPet.Services.Interfaces;

namespace SSPet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadingFilesController : ControllerBase
    {
        private readonly string _filesDirectory;
        public LoadingFilesController(IConfiguration configuration)
        {
            _filesDirectory = configuration["FilesDirectory"];
        }
        [HttpGet("GetStreamFiles")]
        public async Task<IActionResult> GetStreamFiles(string fileName)
        {
            string fullPath = Path.Combine(String.Format(_filesDirectory, 1), fileName);

            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound();
            }

            var fileStream = System.IO.File.OpenRead(fullPath);

            var fileExtension = Path.GetExtension(fullPath).ToLowerInvariant();
            string contentType;

            if (fileExtension == ".m3u8")
            {
                contentType = "application/x-mpegURL";
            }
            else if (fileExtension == ".ts")
            {
                contentType = "video/MP2T";
            }
            else
            {
                return NotFound();
            }

            return new FileStreamResult(fileStream, contentType);
        }
    }
}
