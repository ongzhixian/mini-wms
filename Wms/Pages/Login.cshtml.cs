using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wms.Models;

namespace Wms.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    public string EmailAddress { get; set; }


    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            ViewData["Alert"] = new BootstrapAlert()
            {
                AlertType = "Secondary",
                Description = "Called OnPost"
            };
            return Page();
        }

        // Simulate valid authentication
        ViewData["Message"] = "Called OnPost";
        

        return RedirectToPage("./Index");
    }
}
