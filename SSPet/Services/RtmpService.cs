using System.Diagnostics;

namespace SSPet.Services
{
    public interface IRtmpService
    {
        void Start();
        void Stop();
    }

    public class RtmpService : IRtmpService
    {
        private Process _ffmpegProcess;

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
                Arguments = $"-listen 1 -i rtmp://localhost:1935/live/stream -c:v copy -c:a copy D:\\Programming\\SSPetLocalStorage\\Test\\output.mp4",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
            };

            _ffmpegProcess.Start();
        }

        public void Stop()
        {
            _ffmpegProcess?.Kill();
        }
    }
}
