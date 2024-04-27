using Microsoft.AspNetCore.Mvc;

namespace SSPet.Services.Interfaces
{
    public interface IFfmpegProcessService
    {
        bool CheckFfmpegProcess(int procId);
        Task<int> FfmpegStartAsync();
        Task<bool> FfmpegStopAsync(int procId);
    }
}
