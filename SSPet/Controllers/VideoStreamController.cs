using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;

namespace SSPet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoStreamController : ControllerBase
    {
        private const string FfmpegPath = @"C:\ffmpeg\bin\ffmpeg.exe"; // Path to ffmpeg executable

        [HttpPost("receive")]
        public async Task<IActionResult> ReceiveStream()
        {
            try
            {
                // Generate unique filename for the incoming stream
                string fileName = $"{Guid.NewGuid()}.mp4";
                string filePath = Path.Combine("wwwroot", "streams", fileName);

                // Receive the video stream from OBS
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    Request.Body.CopyTo(stream);
                }

                // Create HLS segments
                string outputDir = Path.Combine("wwwroot", "hls", "output");
                string outputManifest = Path.Combine(outputDir, "index.m3u8");

                // Generate HLS manifest file
                GenerateHlsManifest(outputDir, outputManifest);

                // Execute ffmpeg command to segment the video
                ProcessStartInfo ffmpegInfo = new ProcessStartInfo
                {
                    FileName = FfmpegPath,
                    Arguments = $"-i {filePath} -c:v copy -map 0 -f hls -hls_time 10 -hls_list_size 0 -hls_segment_filename {outputDir}\\segment_%03d.ts {outputManifest}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process ffmpegProcess = Process.Start(ffmpegInfo))
                {
                    ffmpegProcess.WaitForExit();
                }


                // Return the HLS manifest URL to the client
                string manifestUrl = $"{Request.Scheme}://{Request.Host}/hls/output/index.m3u8";
                return Ok(new { ManifestUrl = manifestUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async void GenerateHlsManifest(string outputDir, string outputManifest)
        {
            // Get list of HLS segment files
            string[] segmentFiles = Directory.GetFiles(outputDir, "segment_*.ts");

            // Write HLS manifest file
            await using (StreamWriter writer = new StreamWriter(outputManifest))
            {
                writer.WriteLine("#EXTM3U");
                writer.WriteLine("#EXT-X-VERSION:3");
                writer.WriteLine("#EXT-X-MEDIA-SEQUENCE:0");
                writer.WriteLine("#EXT-X-TARGETDURATION:10");
                foreach (string segmentFile in segmentFiles)
                {
                    string segmentName = Path.GetFileName(segmentFile);
                    writer.WriteLine($"#EXTINF:10.000,");
                    writer.WriteLine($"segment_{segmentName.Substring(8)}");
                }
                writer.WriteLine("#EXT-X-ENDLIST");
            }
        }
    }
}
