using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Wms;

internal static class AppStartup
{
    internal static void SetupAntiForgery(IServiceCollection services)
    {
        services.AddAntiforgery(opts => opts.Cookie.Name = "02884936-6647-410e-a1f8-1ec2be501c36");
    }

    internal static void SetupSession(IServiceCollection services)
    {

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(20);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.Name = "55cf476f-03ba-4fee-86b1-a27f3b135ccc";
        });
    }

    internal static void SetupAuthentication(IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                //options.Events.OnRedirectToAccessDenied =
                //options.Events.OnRedirectToLogin = c =>
                //{
                //    c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                //    return Task.CompletedTask;
                //};

                options.AccessDeniedPath = new PathString("/Account/AccessDenied");
                options.Cookie.Name = "af1ab955-b2ef-423c-8395-f51b58f09ba3";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
                options.LoginPath = new PathString("/Login");
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;

            });
    }

    internal static void SetupAuthorization(IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            //options.AddPolicy("RequireAdministratorRole",
            //     policy => policy.RequireRole("Administrator"));

            //options.AddPolicy("AuthorizedSignalR", policy =>
            //{
            //    policy.AddAuthenticationSchemes(new string[]
            //    {
            //        CookieAuthenticationDefaults.AuthenticationScheme
            //    });
            //    policy.RequireAuthenticatedUser();
            //});

            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(
                    CookieAuthenticationDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

        });
    }

    internal static void SetupHttpClient(ConfigurationManager configuration, IServiceCollection services)
    {
        services.AddHttpClient(); // Add IHttpClientFactory

        services.AddHttpClient("authenticatedClient", (services, http) =>
        {
            IHttpContextAccessor httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();

            if ((httpContextAccessor.HttpContext != null) && httpContextAccessor.HttpContext.Session.Keys.Contains("JWT"))
            {
                string token = httpContextAccessor.HttpContext.Session.GetString("JWT") ?? throw new NullReferenceException("Session[JWT] is null");
                http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

        });
    }

    internal static void SetupServices(ConfigurationManager configuration, IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddDistributedMemoryCache();

        //builder.Services.AddHealthChecks()
        //    .AddCheck<SampleHealthCheck>("Sample")
        //    .AddCheck<SampleHealthCheck2>("Sample2");

        // Add services to the container.
        services.AddRazorPages(options =>
        {
            //options.Conventions.AuthorizePage("/Contact");
            //options.Conventions.AuthorizeFolder("/Private");
            options.Conventions.AllowAnonymousToPage("/Login");
            options.Conventions.AllowAnonymousToFolder("/Public");

            
        });

        //services.Configure<RazorViewEngineOptions>(o =>
        //{
        //    //o.ViewLocationFormats.Clear();
        //    // Public partials
        //    //o.ViewLocationFormats.Add("/Partials/{1}/{0}" + RazorViewEngine.ViewExtension);
        //    o.ViewLocationFormats.Add("/Partials/Public/{0}" + RazorViewEngine.ViewExtension);
        //});

    }

}
