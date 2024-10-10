using dotenv.net;
using Microsoft.AspNetCore.Authentication.Google;
using GestordeTareas.UI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables de entorno desde .env.local
var envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env.local");
DotEnv.Load(new DotEnvOptions(envFilePaths: new[] { envFilePath }));

// Obtén la cadena de conexión de la base de datos desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("GestordeTareasUIContextConnection")
                        ?? throw new InvalidOperationException("Connection string 'GestordeTareasUIContextConnection' not found.");

// Configura el DbContext para usar SQL Server
builder.Services.AddDbContext<GestordeTareasUIContext>(options =>
    options.UseSqlServer(connectionString));

// Configura Identity con confirmación de cuenta
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<GestordeTareasUIContext>();

// Configuración de servicios de autenticación y autorización
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = new PathString("/Usuario/Login");
    options.AccessDeniedPath = new PathString("/home/index");
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
})
.AddGoogle(googleOptions =>
{
    // Cargar las variables de entorno para Google OAuth
    googleOptions.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID")
                            ?? throw new InvalidOperationException("Google Client ID not found in environment variables.");
    googleOptions.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET")
                                ?? throw new InvalidOperationException("Google Client Secret not found in environment variables.");
    googleOptions.CallbackPath = "/signin-google"; // Debe coincidir con la configuración en la consola de Google.
});

// Agregar los controladores con vistas
builder.Services.AddControllersWithViews();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configuración del middleware para el manejo de errores y seguridad
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Valor por defecto para HSTS (30 días)
}

// Middleware de seguridad
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Importante: UseAuthentication debe ir antes de UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

// Configurar las rutas de los controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
