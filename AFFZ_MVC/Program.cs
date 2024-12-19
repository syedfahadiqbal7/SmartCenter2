using AFFZ_Customer.Models;
using AFFZ_Customer.Utils;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc.Razor;
using SCAPI.ServiceDefaults;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.AddServiceDefaults();
//builder.AddDefaultHealthChecks();
builder.Services.AddControllersWithViews();
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Clear();
    options.ViewLocationFormats.Add("/Views/Pages/{0}.cshtml"); // Custom view path
    options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml"); // Shared views path
    options.ViewLocationFormats.Add("/Views/{0}.cshtml"); // Shared views path
                                                          // Add more custom paths as needed
});
var sharedConfig = new ConfigurationBuilder()
    .AddJsonFile(builder.Configuration["SharedFileLocation"].ToString(), optional: false, reloadOnChange: true)
    .Build();

var baseIP = sharedConfig["BaseIP"];
var apiHttpPort = sharedConfig["Ports:AFFZ_API:Http"];
var apiHttpsPort = sharedConfig["Ports:AFFZ_API:Https"];

var CustomerHttpPort = sharedConfig["Ports:AFFZ_Customer:Http"];
var CustomerHttpsPort = sharedConfig["Ports:AFFZ_Customer:Https"];

var AdminHttpPort = sharedConfig["Ports:AFFZ_Admin:Http"];
var AdminHttpsPort = sharedConfig["Ports:AFFZ_Admin:Https"];

var ProviderHttpPort = sharedConfig["Ports:AFFZ_Provider:Http"];
var ProviderHttpsPort = sharedConfig["Ports:AFFZ_Provider:Https"];

var WebFrontHttpPort = sharedConfig["Ports:SCAPI_WebFront:Http"];
var WebFrontHttpsPort = sharedConfig["Ports:SCAPI_WebFront:Https"];

var CertificateName = sharedConfig["Certificate:Path"];
var CertificatePassword = sharedConfig["Certificate:Password"];

// Define configuration programmatically
builder.Services.Configure<AppSettings>(options =>
{
    options.BaseIpAddress = baseIP;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
            $"https://{baseIP}:{CustomerHttpsPort}",
            $"https://{baseIP}:{ProviderHttpsPort}",
            $"https://{baseIP}:{WebFrontHttpsPort}",
            $"https://{baseIP}:{AdminHttpsPort}",
            $"https://{baseIP}:{apiHttpsPort}"
        // Add other client URLs here as needed
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials(); // Required to allow credentials
    });
});
// Add SignalR client services if needed


builder.Services.AddDataProtection();
// Configure cookies
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.HttpOnly = HttpOnlyPolicy.None; // Ensure HttpOnly is not enforced
    options.Secure = CookieSecurePolicy.Always; // Enforce HTTPS
});

// Add services to the container.

// Add session services (if needed)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "SmartCenter";
});


// Add services to the container.
builder.Services.AddHttpClient("Main", client =>
{
    client.BaseAddress = new Uri($"https://{baseIP}:{apiHttpsPort}/api/"); // Replace with your base URL
                                                                           // Additional configuration like headers, timeouts, etc., can be set here
});
builder.Services.AddHttpClient<NotificationService>();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
};
builder.WebHost.UseKestrel(options =>
{
    options.Listen(IPAddress.Parse(baseIP), Convert.ToInt32(CustomerHttpPort));
    options.Listen(IPAddress.Parse(baseIP), Convert.ToInt32(CustomerHttpsPort), listenOptions =>
    {
        listenOptions.UseHttps(CertificateName, CertificatePassword);
    });
});
builder.Services.AddSingleton<IAppSettingsService, AppSettingsService>();
var app = builder.Build();
app.UseCors("AllowSpecificOrigins");
var loggerFactory = app.Services.GetService<ILoggerFactory>();

loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseSessionExpiryMiddleware();
// Use custom middleware
//app.UseMiddleware<RedirectIfNotAuthenticatedMiddleware>();
app.UseRouting();

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");
app.Use(async (context, next) =>
{
    context.Items["BaseIP"] = baseIP;
    await next();
});
app.Run();
