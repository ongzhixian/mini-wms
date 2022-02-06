using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mini.Common.Responses;
using System.IdentityModel.Tokens.Jwt;
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

    private readonly JwtTokenService jwtTokenService;

    public LoginModel(ILogger<LoginModel> logger, UserService userService, JwtTokenService jwtTokenService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ViewData["Alert"] = new BootstrapAlert()
            {
                AlertType = "danger",
                Description = "Invalid form."
            };
            return Page();
        }

#pragma warning disable S125 // Sections of code should not be commented out

        // Simulate valid authentication
        //ViewData["Message"] = new BootstrapAlert()
        //{
        //    AlertType = "secondary",
        //    Description = "OK form."
        //};

        //HttpContext.Session.SetString(SessionKeyName.JWT, loginResponse.Jwt);

        //await userService.GetClaimsPrincipalAsync(loginResponse.Jwt);

        //ClaimsPrincipal claimsPrincipal = await jwtTokenService.GetClaimsPrincipalAsync(loginResponse.Jwt);

        //JwtSecurityToken? jwtSecurityToken = jwtTokenService.Parse(loginResponse.Jwt);

        //ClaimsPrincipal cp = await userService.GetClaimsPrincipalAsync(Login.Username, Login.Password);

        //if (HttpContext != null)
        //{
        //    HttpContext.Session.SetString("JWT", result.Payload.Jwt);

        //    await HttpContext.SignInAsync(
        //    CookieAuthenticationDefaults.AuthenticationScheme,
        //    new ClaimsPrincipal(claimsIdentity),
        //    authProperties);
        //}

#pragma warning restore S125 // Sections of code should not be commented out

        LoginResponse loginResponse = await userService.AuthenticateAsync(Login);


        var (claimsPrincipal, newJwt) = await jwtTokenService.GetSecurityAsync(loginResponse.Jwt);

        if (claimsPrincipal?.Identity?.IsAuthenticated == true)
        {
            HttpContext.Session.SetString(SessionKeyName.JWT, newJwt);

            AuthenticationProperties authenticationProperties = new()
            {
                RedirectUri = "/"
            };

            return SignIn(claimsPrincipal, authenticationProperties,
                CookieAuthenticationDefaults.AuthenticationScheme);

        }

        ViewData["Message"] = new BootstrapAlert()
        {
            AlertType = "danger",
            Description = "Invalid credentials."
        };

        return new OkResult();
    }
}
