using Microsoft.EntityFrameworkCore;
using AppointmentSystem.Data;
using AppointmentSystem.Models;
using Microsoft.AspNetCore.Identity;
using Serilog;
using AppointmentSystem.Handlers.Login;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection;
using AppointmentSystem.Handlers.Admin.Handlers;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppointmentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppointmentSystemConnectionString")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppointmentDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File(Path.Combine(builder.Environment.WebRootPath, "AppointmentSystem.Logs", "app-log.txt"),
        rollingInterval: RollingInterval.Day, 
      
        shared: true)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(LoginHandler).Assembly,
    typeof(DeleteAdminHandler).Assembly
));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var logger = services.GetRequiredService<ILogger<DbInitializer>>();
    await DbInitializer.SeedDataAsync(userManager, roleManager, logger);
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.Use(async (context, next) =>
{
    var sessionUser = context.Session.GetString("Role");

    if (!string.IsNullOrEmpty(sessionUser))
    {
        var currentPath = context.Request.Path.Value.ToLower();

        bool isAdminArea = currentPath.StartsWith("/admin") || currentPath.StartsWith("/managedoctor") || currentPath.StartsWith("/account");
        bool isDoctorArea = currentPath.StartsWith("/doctor")|| currentPath.StartsWith("/account");
        bool isPatientArea = currentPath.StartsWith("/patient")||currentPath.StartsWith("/account");

        if ((sessionUser == "SuperAdmin" || sessionUser == "Admin") && !isAdminArea)
        {
            context.Response.Redirect("/Admin/Dashboard");
            return;
        }
        if (sessionUser == "Doctor" && !isDoctorArea)
        {
            context.Response.Redirect("/Doctor/Dashboard");
            return;
        }
        if (sessionUser == "Patient" && !isPatientArea)
        {
            context.Response.Redirect("/Patient/Dashboard");
            return;
        }
    }

    await next();
});




app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
app.UseResponseCaching();
app.Use(async (context, next) =>
{
    context.Response.Headers[HeaderNames.CacheControl] = "no-store, no-cache, must-revalidate, max-age=0";
    context.Response.Headers[HeaderNames.Pragma] = "no-cache";
    context.Response.Headers[HeaderNames.Expires] = "-1";
    await next();
});
app.Run();
