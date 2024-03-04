using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;

namespace SSPet.Services
{
    public interface IRtmpService
    {
        void Start();
        void Stop();
    }

    public class RtmpService : IRtmpService
    {
        private Process? _ffmpegProcess;

        private readonly IMemoryCache _cache;

        public RtmpService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Start()
        {
            var ffmpegPath = "C:\\ffmpeg\\bin\\ffmpeg.exe";
            var inputUrl = "rtmp://localhost:1935/live/stream"; // OBS RTMP output URL
            var outputUrl = "D:\\Programming\\SSPetLocalStorage\\Test\\output.mp4"; // Destination file or processing endpoint

            _ffmpegProcess = new Process
            {
                StartInfo =
            {
                FileName = ffmpegPath,
                Arguments = $"-listen 1 -i rtmp://localhost:1935/live/stream -c:v copy -c:a copy -f hls -hls_time 5 -hls_list_size 0 -hls_segment_filename D:\\Programming\\SSPetLocalStorage\\Test\\output_%03d.ts  D:\\Programming\\SSPetLocalStorage\\Test\\output.m3u8",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
            };


            _ffmpegProcess.Start();
            _cache.Set("Stream1", _ffmpegProcess.Id);
        }

        public void Stop()
        {
            int idToKill = -1;
            if (_cache.TryGetValue("Stream1", out int cachedValue)) idToKill = cachedValue;
            Process streamToKill = Process.GetProcessById(idToKill);
            streamToKill.Kill();
            _cache.Remove("Stream1");
        }
    }
}
