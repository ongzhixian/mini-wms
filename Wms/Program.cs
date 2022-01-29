using Wms;

var builder = WebApplication.CreateBuilder(args);

AppStartup.SetupAntiForgery(builder.Services);

AppStartup.SetupSession(builder.Services);

AppStartup.SetupAuthentication(builder.Services);

AppStartup.SetupAuthorization(builder.Services);

AppStartup.SetupHttpClient(builder.Configuration, builder.Services);

AppStartup.SetupServices(builder.Configuration, builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();
