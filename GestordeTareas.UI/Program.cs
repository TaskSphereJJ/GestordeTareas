using Microsoft.AspNetCore.Authentication.Google;
using GestordeTareas.UI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("GestordeTareasUIContextConnection") ?? throw new InvalidOperationException("Connection string 'GestordeTareasUIContextConnection' not found.");

builder.Services.AddDbContext<GestordeTareasUIContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<GestordeTareasUIContext>();
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<EmailService>();

// configurar la autenticaci�n
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
    AddCookie((options) =>
    {

        options.LoginPath = new PathString("/Usuario/Login");
        options.AccessDeniedPath = new PathString("/home/index");
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;


    });

var app = builder.Build();
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

app.UseAuthentication(); // poner en uso la autenticaci�n

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();