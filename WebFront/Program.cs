using Microsoft.AspNetCore.CookiePolicy;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Configure services
builder.Services.AddDataProtection();

// Configure cookies
builder.Services.Configure<CookiePolicyOptions>(options =>
{
	options.MinimumSameSitePolicy = SameSiteMode.None;
	options.HttpOnly = HttpOnlyPolicy.None; // Ensure HttpOnly is not enforced
	options.Secure = CookieSecurePolicy.Always; // Enforce HTTPS
});

// Add distributed SQL server cache for session storage
builder.Services.AddDistributedSqlServerCache(options =>
{
	options.ConnectionString = builder.Configuration.GetConnectionString("DBCS");
	options.SchemaName = "dbo";
	options.TableName = "SessionData";
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
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAllOrigins",
		builder => builder.AllowAnyOrigin()
						  .AllowAnyMethod()
						  .AllowAnyHeader());
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
app.UseCors("AllowAllOrigins"); // Applies CORS policy
app.UseAuthorization(); // Adds authorization middleware

// Configure endpoints
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();