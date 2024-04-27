using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SSPet.Services.Interfaces;

namespace SSPet.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IRtmpService _rtmpService;

        public IndexModel(ILogger<IndexModel> logger, IRtmpService rtmpService)
        {
            _logger = logger;
            _rtmpService = rtmpService;
        }

        [BindProperty]
        public int Id { get; set; } = 1;

        public string Result { get; set; }

        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPostStartStreamingAsync(int id)
        {
            Result = await _rtmpService.StartAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostStopStreamingAsync(int id)
        {
            Result = await _rtmpService.StopAsync();
            return Page();
        }
    }
}