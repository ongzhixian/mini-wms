using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Wms.Models;
using Wms.Services;

namespace Wms.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    public LoginViewModel Login { get; set; } = new LoginViewModel();

    private readonly ILogger<LoginModel> logger;

    private readonly UserService userService;

    public LoginModel(ILogger<LoginModel> logger, UserService userService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
    }

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
        
        ClaimsPrincipal cp = await userService.GetClaimsPrincipalAsync(Login.Username, Login.Password);

        return this.SignIn(cp, CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
