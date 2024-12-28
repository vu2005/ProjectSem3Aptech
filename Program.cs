using CarInsuranceManage.Database;
using CarInsuranceManage.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Đăng ký dịch vụ nền
builder.Services.AddHostedService<InsuranceStatusService>();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddSingleton<EmailService>();
// Register HttpClientFactory
builder.Services.AddHttpClient();
builder.Services.AddSingleton<PayPalService>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    return new PayPalService(configuration);
});

builder.Services.AddDbContext<CarInsuranceDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


// // Configure DbContext for MySQL
// builder.Services.AddDbContext<CarInsuranceDbContext>(options =>
//     options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
//                      new MySqlServerVersion(new Version(8, 0, 33))));  // Ensure correct MySQL version

// Configure authentication services (Google and Facebook)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;  // Default is Google
})
.AddCookie()  // Add Cookie Authentication
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
})
.AddFacebook(options =>
{
    options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
});

// Add session management
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Apply migrations to the DB if necessary
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CarInsuranceDbContext>();
    try
    {
        dbContext.Database.Migrate();  // Apply migrations and seed data if needed
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);  // Log error if any
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();  // Use HTTP Strict Transport Security
}

app.UseHttpsRedirection();
app.UseStaticFiles();  // Serve static files (CSS, JS, images)

app.UseRouting();

// Use session management middleware
app.UseSession();

// Use authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Configure default route pattern for controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
