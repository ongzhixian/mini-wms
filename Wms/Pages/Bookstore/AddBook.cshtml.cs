using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wms.Models.Data.Bookstore;
using Wms.Services;

namespace Wms.Pages.Bookstore;

public class AddBookModel : PageModel
{
    [BindProperty]
    public Book? Book { get; set; }

    private readonly BookService bookService;

    public AddBookModel(BookService bookService)
    {
        this.bookService = bookService;
        this.Book = new Book();
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        //_context.Movie.Add(Movie);
        //await _context.SaveChangesAsync();

        if (Book != null)
            await bookService.CreateAsync(Book);

        return RedirectToPage("/Bookstore/Index");
    }
}
