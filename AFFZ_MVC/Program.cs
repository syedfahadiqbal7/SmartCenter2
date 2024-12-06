using AFFZ_Customer.Utils;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Clear();
    options.ViewLocationFormats.Add("/Views/Pages/{0}.cshtml"); // Custom view path
    options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml"); // Shared views path
    options.ViewLocationFormats.Add("/Views/{0}.cshtml"); // Shared views path
                                                          // Add more custom paths as needed
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
    builder =>
    {
        builder.WithOrigins("https://localhost:7195", "https://localhost:7096")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
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
    client.BaseAddress = new Uri("https://localhost:7047/api/"); // Replace with your base URL
                                                                 // Additional configuration like headers, timeouts, etc., can be set here
});
builder.Services.AddHttpClient<NotificationService>();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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

app.Run();
