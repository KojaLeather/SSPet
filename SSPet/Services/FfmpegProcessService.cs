using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SSPet.Models;
using SSPet.Services.Interfaces;

namespace SSPet.Services
{
    public class FfmpegProcessService : IFfmpegProcessService
    {
        private Process? _ffmpegProcess;
        private readonly IConfiguration _configuration;
        public FfmpegProcessService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool CheckFfmpegProcess(int procId)
        {
            try
            {
                Process checkFfmpeg = Process.GetProcessById(procId);
                if (checkFfmpeg.ProcessName == "ffmpeg") return true;
                else return false;
            }
            catch (InvalidOperationException ex)
            {
                return false;
            }
        }
        public async Task<int> FfmpegStartAsync()
        {
            FfmpegSettings settings = new FfmpegSettings();
            _configuration.GetSection(FfmpegSettings.Key).Bind(settings);
            _ffmpegProcess = new Process
            {
                StartInfo =
                {
            FileName = settings.FfmpegPath,
            Arguments = $"-listen 1 -i {string.Format(settings.InputUrl, 1)} -c:v copy -c:a copy -f hls -hls_time 5" +
                        $" -hls_list_size 0 -hls_segment_filename {string.Format(settings.OutputTsUrl, 1)}" +
                        $" -hls_base_url https://localhost:7239/api/LoadingFiles/GetStreamFiles?fileName=" +
            $" {string.Format(settings.OutputManifestUrl, 1)}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
                }
            };
            await Task.Run(() => _ffmpegProcess.Start());
            return _ffmpegProcess.Id;
        }
        public async Task<bool> FfmpegStopAsync(int procId)
        {

            if (CheckFfmpegProcess(procId))
            {
                Process streamToKill = Process.GetProcessById(procId);
                await Task.Run(() =>
                {
                    streamToKill.Kill();
                    streamToKill.WaitForExitAsync();
                });
                return true;
            }
            else return false;
        }
    }
}
