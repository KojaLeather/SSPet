using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;

namespace SSPet.Services
{
    public interface IRtmpService
    {
        string Start(int id);
        string Stop(int id);
    }

    public class RtmpService : IRtmpService
    {
        private Process? _ffmpegProcess;

        private readonly IMemoryCache _cache;

        public RtmpService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string Start(int id)
        {
            if (_cache.TryGetValue($"Stream{id}", out int cachedValue))
            {
                try
                {
                    Process checkFfmpeg = Process.GetProcessById(cachedValue);
                    if (checkFfmpeg.ProcessName == "ffmpeg") return $"There is existing ffmpeg process on {id} key.";
                    else _cache.Remove($"Stream{id}");
                }
                catch (InvalidOperationException ex)
                {
                    _cache.Remove($"Stream{id}");
                }
            }
            var ffmpegPath = "C:\\ffmpeg\\bin\\ffmpeg.exe";
            var inputUrl = $"rtmp://localhost:1935/live/stream/{id}";
            var outputTsUrl = $"D:\\Programming\\SSPetLocalStorage\\{id}\\output_%03d.ts";
            var outputManifestUrl = $"D:\\Programming\\SSPetLocalStorage\\{id}\\output.m3u8"; 

            _ffmpegProcess = new Process
            {
                StartInfo =
            {
                FileName = ffmpegPath,
                Arguments = $"-listen 1 -i {inputUrl} -c:v copy -c:a copy -f hls -hls_time 5 -hls_list_size 0 -hls_segment_filename {outputTsUrl}  {outputManifestUrl}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
            };


            _ffmpegProcess.Start();
            _cache.Set($"Stream{id}", _ffmpegProcess.Id);
            return $"Stream successfuly started on {id} key. Use this url for connection: {inputUrl}";
        }

        public string Stop(int id)
        {
            int idToKill = -1;
            if (_cache.TryGetValue($"Stream{id}", out int cachedValue))
            {
                idToKill = cachedValue;
                Process streamToKill = Process.GetProcessById(idToKill);
                streamToKill.Kill();
                _cache.Remove($"Stream{id}");
                return $"Stream successfully stopped on {id} key";
            }
            else return $"Stopping stream on {id} key failed, there is no PID saved in cache";
        }
    }
}
