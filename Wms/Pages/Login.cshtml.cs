using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mini.Common.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Wms.Models;
using Wms.Models.Notifications;
using Wms.Services;

namespace Wms.Pages;

public class LoginModel : PageModel
{
    [BindProperty]
    public LoginViewModel Login { get; set; } = new LoginViewModel();

    private readonly ILogger<LoginModel> logger;

    private readonly IUserService userService;

    private readonly IJwtTokenService jwtTokenService;

    private readonly IUserProfileService userProfileService;

    public LoginModel(ILogger<LoginModel> logger, IUserService userService, IJwtTokenService jwtTokenService, IUserProfileService userProfileService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        this.jwtTokenService = jwtTokenService ?? throw new ArgumentNullException(nameof(jwtTokenService));
        this.userProfileService = userProfileService ?? throw new ArgumentNullException(nameof(userProfileService));
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ViewData["Notification"] = new ErrorNotification("Invalid form.");
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

            var userProfile = userProfileService.GetUserProfileAsync(claimsPrincipal.Identity?.Name);

            AuthenticationProperties authenticationProperties = new()
            {
                RedirectUri = GetRedirectUri(userProfile)
            };

            return SignIn(claimsPrincipal, authenticationProperties,
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        ViewData["Notification"] = new BadRequestNotification("Invalid credentials.");

        return Page();
    }

    private string GetRedirectUri(Models.Shared.UserProfile userProfile)
    {
        //return Request.Query["ReturnUrl"].Count == 0 ? "/" : Request.Query["ReturnUrl"].ToString();

        if (Request.Query["ReturnUrl"].Count > 0)
        {
            return Request.Query["ReturnUrl"].ToString();
        }

        // If no ReturnUrl, we use userProfile.PreferredApplication if available else default to root
        if (string.IsNullOrWhiteSpace(userProfile.PreferredApplication))
        {
            return "/";
        }
        else
        {
            return userProfile.PreferredApplication;
        }
    }
}
