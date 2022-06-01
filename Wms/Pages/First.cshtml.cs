using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Wms.Pages;

public class FirstModel : PageModel
{
    private readonly ILogger<FirstModel> _logger;

    public FirstModel(ILogger<FirstModel> logger)
    {
        _logger = logger;
    }
}
