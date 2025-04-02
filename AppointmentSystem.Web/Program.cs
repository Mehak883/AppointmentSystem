using Microsoft.EntityFrameworkCore;
using AppointmentSystem.Data;
using AppointmentSystem.Models;
using Microsoft.AspNetCore.Identity;
using Serilog;
using AppointmentSystem.Handlers.Login;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Reflection;
using AppointmentSystem.Handlers.Admin.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppointmentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppointmentSystemConnectionString")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppointmentDbContext>()
    .AddDefaultTokenProviders();

// Register IHttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Authentication and Cookie Configurations
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
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Make it consistent with authentication
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .WriteTo.File("AppointmentSystem.Logs/app-log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Register MediatR in a single call
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
    typeof(LoginHandler).Assembly,
    typeof(DeleteAdminHandler).Assembly
));

var app = builder.Build();

// Seed roles & users
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var logger = services.GetRequiredService<ILogger<DbInitializer>>();
    await DbInitializer.SeedDataAsync(userManager, roleManager, logger);
}

// Configure middleware
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


//app.Use(async (context, next) =>
//{
//    var sessionUser = context.Session.GetString("Role");

//    if (!string.IsNullOrEmpty(sessionUser))
//    {
//        var currentPath = context.Request.Path.Value.ToLower();

//        // Prevent infinite loops
//        if (!(currentPath.StartsWith("/admin/dashboard") && sessionUser == "SuperAdmin") &&
//            !(currentPath.StartsWith("/doctor/dashboard") && sessionUser == "Doctor") &&
//            !(currentPath.StartsWith("/patient/profile") && sessionUser == "Patient"))
//        {
//            switch (sessionUser)
//            {
//                case "SuperAdmin":
//                case "Admin":
//                    context.Response.Redirect("/Admin/Dashboard");
//                    return;
//                case "Doctor":
//                    context.Response.Redirect("/Doctor/Dashboard");
//                    return;
//                case "Patient":
//                    context.Response.Redirect("/Patient/Profile");
//                    return;
//            }
//        }
//    }



//await next();
//});

// Session-based role redirection middleware
//app.Use(async (context, next) =>
//{
//    //var sessionUser = "SuperAdmin";
//    var sessionUser = context.Session.GetString("Role");

//    if (string.IsNullOrEmpty(sessionUser))
//    {
//        context.Response.Redirect("/Account/Login");
//        return;
//    }

//    await next(); // Ensure next middleware gets executed before redirection

//    switch (sessionUser)
//    {
//        case "SuperAdmin":
//            context.Response.Redirect("/Admin/Dashboard");
//            return;
//        case "Doctor":
//            context.Response.Redirect("/Doctor/Dashboard");
//            return;
//        case "Patient":
//            context.Response.Redirect("/Patient/Dashboard");
//            return;
//        default:
//            context.Response.Redirect("/Account/Login");
//            return;
//    }
//});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
