﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Mini.Common.Services;
using Mini.Common.Settings;
using Wms.DbContexts;
using Wms.Models;
using Wms.Services;
using Wms.Services.HttpClients;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using MongoDB.Driver;

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
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
                options.LoginPath = new PathString("/Login");
                options.LogoutPath = new PathString("/Logout");
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter; // Default is ?ReturnUrl=%2F
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

    internal static void SetupDatabaseContexts(ConfigurationManager configuration, IServiceCollection services)
    {
        // To do something about "UseSqlite", we should not bind variable code
        services.AddDbContext<LocalContext>(options =>
            options.UseSqlite(ResolveSqliteDbConnectionString(configuration.GetConnectionString("LocalContext"))
        ));

        // To do something about "UseSqlite", we should not bind variable code
        services.AddDbContext<BloggingContext>(options =>
            options.UseSqlite(ResolveSqliteDbConnectionString(configuration.GetConnectionString("BloggingContext"))
        ));

        services.AddDbContext<SchoolContext>(options =>
            options.UseSqlite(ResolveSqliteDbConnectionString(configuration.GetConnectionString("SchoolContext"))
        ));

        services.AddDbContext<AgileContext>(options =>
            options.UseSqlite(ResolveSqliteDbConnectionString(configuration.GetConnectionString("AgileContext"))
        ));

        // Code snippet for use with SqlServer
        //services.AddDbContext<SchoolContext>(
        //    options => options.UseSqlServer(
        //        configuration.GetConnectionString("SchoolContext")));

        // services.AddSingleton<IMongoDatabase>(sp =>
        // {
        //     string miniToolsConnectionString = configuration.GetValue<string>("mongodb:minitools:ConnectionString");
        //     IMongoClient mongoClient = new MongoClient(miniToolsConnectionString);
        //     string databaseName = MongoUrl.Create(miniToolsConnectionString).DatabaseName;
        //     return mongoClient.GetDatabase(databaseName);
        // });

        // services.AddSingleton<IMongoCollection<User>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<User>("user"));
        // services.AddSingleton<IMongoCollection<Bookmark>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<Bookmark>("bookmark"));
        // services.AddSingleton<IMongoCollection<Note>>(sp => sp.GetRequiredService<IMongoDatabase>().GetCollection<Note>("note"));

        // Example of factory
        //builder.Services.AddDbContextFactory<ContactContext>(opt =>
        //    opt.UseSqlite($"Data Source={nameof(ContactContext.ContactsDb)}.db"));
        // See: https://docs.microsoft.com/en-us/aspnet/core/blazor/blazor-server-ef-core?view=aspnetcore-6.0

        services.AddDatabaseDeveloperPageExceptionFilter();
    }

    internal static void SetupHttpClient(ConfigurationManager configuration, IServiceCollection services)
    {
        // Add IHttpClientFactory
        services.AddHttpClient();

        //services.AddHttpClient<AuthenticationHttpClient>((services, http) =>
        //{
        //    IHttpContextAccessor httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();

        //    IOptionsMonitor<HttpClientSetting> optionsMonitor = services.GetRequiredService<IOptionsMonitor<HttpClientSetting>>();

        //    HttpClientSetting? httpClientSetting = optionsMonitor.Get(HttpClientName.Authentication);

        //    httpClientSetting.EnsureIsValid();

        //    http.BaseAddress = new Uri(httpClientSetting.BaseAddress);

        //    if ((httpContextAccessor.HttpContext != null) && httpContextAccessor.HttpContext.Session.Keys.Contains("JWT"))
        //    {
        //        string token = httpContextAccessor.HttpContext.Session.GetString("JWT") ?? throw new NullReferenceException("Session[JWT] is null");

        //        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        //    }
        //});

        // Using named -- httpClientFactory.CreateClient(HttpClientName.Authentication)

        //services.AddHttpClient(HttpClientName.Authentication, (services, http) =>
        //{
        //    IHttpContextAccessor httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();

        //    IOptionsMonitor<HttpClientSetting> optionsMonitor = services.GetRequiredService<IOptionsMonitor<HttpClientSetting>>();

        //    HttpClientSetting? httpClientSetting = optionsMonitor.Get(HttpClientName.Authentication);

        //    httpClientSetting.EnsureIsValid();

        //    http.BaseAddress = new Uri(httpClientSetting.BaseAddress);

        //    if ((httpContextAccessor.HttpContext != null) && httpContextAccessor.HttpContext.Session.Keys.Contains("JWT"))
        //    {
        //        string token = httpContextAccessor.HttpContext.Session.GetString("JWT") ?? throw new NullReferenceException("Session[JWT] is null");

        //        http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        //    }
        //});

        services.AddHttpClient(HttpClientName.BearerHttpClient, (services, http) =>
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
        services.Configure<RsaKeySetting>(RsaKeyName.RecepSigningKey, configuration.GetSection(RsaKeyName.RecepSigningKey));
        services.Configure<RsaKeySetting>(RsaKeyName.RecepEncryptingKey, configuration.GetSection(RsaKeyName.RecepEncryptingKey));
        services.Configure<RsaKeySetting>(RsaKeyName.EncryptingKey, configuration.GetSection(RsaKeyName.EncryptingKey));

        services.Configure<HttpClientSetting>(HttpClientName.AuthenticationEndpoint, configuration.GetSection(HttpClientName.AuthenticationEndpoint));
        services.Configure<HttpClientSetting>(HttpClientName.UserEndpoint, configuration.GetSection(HttpClientName.UserEndpoint));

        services.AddHttpContextAccessor();

        services.AddDistributedMemoryCache();

        //builder.Services.AddHealthChecks()
        //    .AddCheck<SampleHealthCheck>("Sample")
        //    .AddCheck<SampleHealthCheck2>("Sample2");

        // Add services to the container.
        services.AddRazorPages(options =>
        {
            //options.Conventions.AuthorizePage("/Contact")
            //options.Conventions.AuthorizeFolder("/Private")

            options.Conventions.AllowAnonymousToPage("/Login");
            options.Conventions.AllowAnonymousToPage("/Logout");
            options.Conventions.AllowAnonymousToPage("/Error");
            options.Conventions.AllowAnonymousToFolder("/Public");

        });

        if (configuration.GetValue<bool>("Application:UseLocal"))
        {
            services.AddScoped<IUserService, LocalUserService>();
            services.AddScoped<IJwtTokenService, LocalJwtTokenService>();
        }
        else
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
        }
            
        services.AddScoped<PkedService>();
        
        services.AddScoped<AuthenticationEndpoint>();
        services.AddScoped<UserEndpoint>();
        services.AddScoped<UserServiceHttpClient>();

    }

    internal static void InitializeDatabases(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;

            InitializeLocalContext(services);

            InitializeBloggingContext(services);

            InitializeSchoolContext(services);
        }
    }

    private static void InitializeSchoolContext(IServiceProvider services)
    {
        var context = services.GetRequiredService<SchoolContext>();

        context.Database.EnsureCreated();

        SchoolContextInitializer.Initialize(context);
    }

    private static void InitializeBloggingContext(IServiceProvider services)
    {
        var context = services.GetRequiredService<BloggingContext>();

        context.Database.EnsureCreated();
    }

    private static void InitializeLocalContext(IServiceProvider services)
    {
        var context = services.GetRequiredService<LocalContext>();

        context.Database.EnsureCreated();
        
        LocalContextInitializer.Initialize(context);
    }

    private static string ResolveSqliteDbConnectionString(string connectionString)
    {
        // The "Data Source" field specifies the location of the database.
        // The location may be a relative (non-rooted) path.
        // To avoid ambiguity, we will resolve relative path in relation to running executable.
        // Example  of relative path: "./Data/asdbFile.sqlite"
        // Examples of absolute path: "/Data/dbFile.sqlite" and "C:/Data/dbFile.sqlite"

        SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder(connectionString);

        if (!Path.IsPathRooted(builder.DataSource))
        {
            var filePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, builder.DataSource));

            Console.WriteLine($"Resolved file path: {filePath}");

            // Create the directory if it does not exists

            var directoryPath = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // To make configurable
            //if (File.Exists(filePath))
            //{
            //    File.Delete(filePath);
            //}

            builder.DataSource = filePath;
        }

        return builder.ToString();
    }
}


public static class HttpClientName
{
    public const string BearerHttpClient = "BearerHttpClient";
    public const string AuthenticationEndpoint = "HttpClients:AuthenticationEndpoint";
    public const string RefreshTokenEndpoint = "HttpClients:RefreshTokenEndpoint";
    public const string UserEndpoint = "HttpClients:UserEndpoint";
    
    
}

public static class RsaKeyName
{
    // Public key only
    public const string RecepSigningKey = "RsaKeys:RecepSigningPublicKey";
    public const string RecepEncryptingKey = "RsaKeys:RecepEncryptingPublicKey";

    // Private key
    public const string EncryptingKey = "RsaKeys:EncryptingKey"; 
}

public static class SessionKeyName
{
    public const string JWT = "JWT";
}
