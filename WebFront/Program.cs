using Microsoft.AspNetCore.CookiePolicy;
using SCAPI.ServiceDefaults;
using SCAPI.WebFront;
using SCAPI.WebFront.Models;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
//builder.AddDefaultHealthChecks();

// Configure services
builder.Services.AddDataProtection();

// Configure cookies
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.HttpOnly = HttpOnlyPolicy.None; // Ensure HttpOnly is not enforced
    options.Secure = CookieSecurePolicy.Always; // Enforce HTTPS
});
// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = "SmartCenter";
});

// Add CORS
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

// Add services to the container.
builder.Services.AddHttpClient("Main", client =>
{
    client.BaseAddress = new Uri($"https://{baseIP}:{apiHttpsPort}/api/"); // Replace with your base URL
                                                                           // Additional configuration like headers, timeouts, etc., can be set here
});

// Add controllers with views
builder.Services.AddControllersWithViews();

// Add logging
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
    logging.AddEventSourceLogger();
});
var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
};
builder.WebHost.UseKestrel(options =>
{
    options.Listen(IPAddress.Parse(baseIP), Convert.ToInt32(WebFrontHttpPort));
    options.Listen(IPAddress.Parse(baseIP), Convert.ToInt32(WebFrontHttpsPort), listenOptions =>
    {
        listenOptions.UseHttps(CertificateName, CertificatePassword);
    });
});

// Define configuration programmatically
builder.Services.Configure<AppSettings>(options =>
{
    options.UserUrl = $"https://{baseIP}:{CustomerHttpsPort}/";
    options.ProviderUrl = $"https://{baseIP}:{ProviderHttpsPort}/";
    options.APIURL = $"https://{baseIP}:{apiHttpsPort}";
});
builder.Services.AddSingleton<WebApiHelper>(); // Register the helper class
builder.Services.AddSingleton<IAppSettingsService, AppSettingsService>();
var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // Enforces HTTP Strict Transport Security (HSTS)
}

app.UseHttpsRedirection(); // Redirects HTTP requests to HTTPS
app.UseStaticFiles(); // Serves static files
app.UseSession(); // Enables session support
app.UseRouting(); // Adds routing to the request pipeline
app.UseCors("AllowSpecificOrigins"); // Applies CORS policy
app.UseAuthorization(); // Adds authorization middleware

// Configure endpoints
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();