using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using SSPet.Services.Interfaces;

namespace SSPet.Services
{

    public class RtmpService : IRtmpService
    {

        private readonly IMemoryCache _cache;
        private readonly IFfmpegProcessService _ffmpegProcessService;
        private readonly IConfiguration _configuration;

        public RtmpService(IMemoryCache cache, IFfmpegProcessService ffmpegProcessService, IConfiguration configuration)
        {
            _cache = cache;
            _ffmpegProcessService = ffmpegProcessService;
            _configuration = configuration;
        }

        public async Task<string> StartAsync()
        {
            if (_cache.TryGetValue("Stream", out int cachedValue))
            {
                if (_ffmpegProcessService.CheckFfmpegProcess(cachedValue))
                {
                    return "Stream on this id is already launched";
                }
                else _cache.Remove("Stream");
            }
            int processId = await _ffmpegProcessService.FfmpegStartAsync(); //Using FfmpegStart service

            _cache.Set("Stream", processId);
            return $"Stream successfuly started on 1 key. Use that key for connection.";
        }

        public async Task<string> StopAsync()
        {
            string path = _configuration["FilesDirectory"];
            System.IO.DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            if (_cache.TryGetValue("Stream", out int cachedValue))
            {
                string response;
                if (await _ffmpegProcessService.FfmpegStopAsync(cachedValue))
                {
                    response = $"Stream successfully stopped on 1 key";
                }
                else
                {
                    response = $"Process saved in cache is not ffmpeg, check process with PID: {cachedValue}";
                }
                _cache.Remove("Stream");
                return response;
            }
            else return $"Stopping stream on 1 key is failed, there is no PID saved in cache";
        }
    }
}
