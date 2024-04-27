namespace SSPet.Services.Interfaces
{
    public interface IRtmpService
    {
        Task<string> StartAsync();
        Task<string> StopAsync();
    }
}
