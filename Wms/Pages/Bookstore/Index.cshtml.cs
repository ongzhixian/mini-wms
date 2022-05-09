using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wms.Services;

namespace Wms.Pages.Bookstore;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly BookService bookService;

    [BindProperty]
    public string Search { get; set; }

    public IndexModel(ILogger<IndexModel> logger, BookService bookService)
    {
        _logger = logger;
        this.bookService = bookService;
    }

    public async Task OnGetAsync()
    {
        // var s = await bookService.GetAsync();
        // Console.WriteLine(s);
    }
}
