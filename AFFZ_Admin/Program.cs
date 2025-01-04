using AFFZ_Admin.Models;
using AFFZ_Admin.Utils;
using Microsoft.AspNetCore.Mvc.Razor;
using SCAPI.ServiceDefaults;
using System.Net;
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
//builder.AddDefaultHealthChecks();
// Add services to the container.
builder.Services.AddControllersWithViews();

var sharedConfig = new ConfigurationBuilder()
    .AddJsonFile(builder.Configuration["SharedFileLocation"].ToString(), optional: false, reloadOnChange: true)
    .Build();

var baseIP = sharedConfig["BaseIP"];
var PublicDomain = sharedConfig["PublicDomain"];
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
    options.PublicDomain = PublicDomain;
    options.ApiHttpsPort = apiHttpsPort;
    options.AdminHttpsPort = AdminHttpsPort;
    options.MerchantHttpsPort = ProviderHttpsPort;
    options.CustomerHttpsPort = CustomerHttpsPort;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
            $"https://{baseIP}:{CustomerHttpsPort}",
            $"https://{baseIP}:{ProviderHttpsPort}",
            $"https://{baseIP}:{WebFrontHttpsPort}",
            $"https://{baseIP}:{AdminHttpsPort}"
        // Add other client URLs here as needed
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials(); // Required to allow credentials
    });
});
builder.Services.AddHttpClient("Main", client =>
{
    client.BaseAddress = new Uri($"https://{baseIP}:{apiHttpsPort}/api/"); // Replace with your base URL
                                                                           // Additional configuration like headers, timeouts, etc., can be set here
});
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Clear();
    options.ViewLocationFormats.Add("/Views/Pages/{0}.cshtml"); // Custom view path
    options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml"); // Shared views path
                                                                 // Add more custom paths as needed
});
var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
};
//builder.WebHost.UseKestrel()
//    .ConfigureAppConfiguration((hostingContext, config) =>
//    {
//        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
//    });
builder.WebHost.UseKestrel(options =>
{

    options.Listen(IPAddress.Parse(baseIP), Convert.ToInt32(AdminHttpPort));
    options.Listen(IPAddress.Parse(baseIP), Convert.ToInt32(AdminHttpsPort), listenOptions =>
    {
        listenOptions.UseHttps(CertificateName, CertificatePassword);
    });
});

builder.Services.AddSingleton<IAppSettingsService, AppSettingsService>();

var app = builder.Build();
var loggerFactory = app.Services.GetService<ILoggerFactory>();

loggerFactory.AddFile(builder.Configuration["Logging:LogFilePath"].ToString());
app.UseCors("AllowSpecificOrigins");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
