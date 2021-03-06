using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Wms.Pages;

public class LogoutModel : PageModel
{
    public IActionResult OnGet()
    {
        return this.SignOut(new AuthenticationProperties
        {
            RedirectUri = "/Index"
        },
        CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public IActionResult OnPost()
    {
        return this.SignOut(new AuthenticationProperties
        {
            RedirectUri = "/Index"
        },
        CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
