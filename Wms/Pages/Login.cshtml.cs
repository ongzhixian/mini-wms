using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Wms.Models;

namespace Wms.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    public LoginViewModel Login { get; set; }


    //public void OnGet()
    //{
    //}

    public async Task<IActionResult> OnPostAsync()
    {
        //if (!ModelState.IsValid)
        //{
        //    ViewData["Alert"] = new BootstrapAlert()
        //    {
        //        AlertType = "danger",
        //        Description = "Invalid form."
        //    };
        //    return Page();
        //}

        // Simulate valid authentication
        ViewData["Message"] = new BootstrapAlert()
        {
            AlertType = "secondary",
            Description = "OK form."
        };

        List<Claim> claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, "someUsername"));

        ClaimsIdentity ci = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        ClaimsPrincipal cp = new ClaimsPrincipal(ci);

        return this.SignIn(cp, CookieAuthenticationDefaults.AuthenticationScheme);

        //await HttpContext.SignInAsync(
        //    CookieAuthenticationDefaults.AuthenticationScheme, cp);
        //return RedirectToPage("/Index");
    }
}
