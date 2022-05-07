using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using Wms.DbContexts;

namespace Wms.Pages.Bookstore;

public class BookManagerModel : PageModel
{

    [BindProperty]
    public long BookCount { get; set; }

    [BindProperty]
    public long AuthorCount { get; set; }

    [BindProperty]
    public long CategoryCount { get; set; }

    private readonly BookstoreContext bookstoreContext;

    public BookManagerModel(BookstoreContext bookstoreContext)
    {
        this.bookstoreContext = bookstoreContext;
    }

    public async Task OnGetAsync()
    {
        BookCount = await bookstoreContext.Books.CountDocumentsAsync(_ => true);
        AuthorCount = await bookstoreContext.Authors.CountDocumentsAsync(_ => true);
        CategoryCount = await bookstoreContext.Categories.CountDocumentsAsync(_ => true);
    }
}
