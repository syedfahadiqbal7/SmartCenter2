using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
    builder =>
    {
        builder.WithOrigins("https://localhost:7195", "https://localhost:7096", "https://localhost:7083")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});
builder.Services.AddHttpClient("Main", client =>
{
    client.BaseAddress = new Uri("https://localhost:7047/api/"); // Replace with your base URL
                                                                 // Additional configuration like headers, timeouts, etc., can be set here
});
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Clear();
    options.ViewLocationFormats.Add("/Views/Pages/{0}.cshtml"); // Custom view path
    options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml"); // Shared views path
                                                                 // Add more custom paths as needed
});
var app = builder.Build();
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
