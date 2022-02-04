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

        LoginResponse loginResponse = await userService.AuthenticateAsync(Login);

        HttpContext.Session.SetString(SessionKeyName.JWT, loginResponse.Jwt);

        await userService.GetClaimsPrincipalAsync(loginResponse.Jwt);

        //ClaimsPrincipal claimsPrincipal = await jwtTokenService.GetClaimsPrincipalAsync(loginResponse.Jwt);

        //string newJwt = await jwtTokenService.GetSecurityAsync(loginResponse.Jwt);

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

        AuthenticationProperties authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = "/"
        };

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();


        return this.SignIn(claimsPrincipal, authenticationProperties,
            CookieAuthenticationDefaults.AuthenticationScheme);
    }
}
