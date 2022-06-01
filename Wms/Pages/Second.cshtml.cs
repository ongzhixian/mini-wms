using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Wms.Pages;

public class SecondModel : PageModel
{
    private readonly ILogger<SecondModel> _logger;

    public SecondModel(ILogger<SecondModel> logger)
    {
        _logger = logger;
    }
}
