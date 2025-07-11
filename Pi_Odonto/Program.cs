using Microsoft.EntityFrameworkCore;
using Pi_Odonto.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

// === CONFIGURA��O DE AUTENTICA��O ===
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";           // Redireciona para login se n�o autenticado
        options.LogoutPath = "/Logout";         // P�gina de logout
        options.AccessDeniedPath = "/Login";    // Redireciona se acesso negado
        options.ExpireTimeSpan = TimeSpan.FromHours(2);    // Sess�o expira em 2 horas
        options.SlidingExpiration = true;       // Renova automaticamente se usuario ativo
        options.Cookie.Name = "PiOdontoAuth";   // Nome do cookie
        options.Cookie.HttpOnly = true;         // Seguran�a - s� server acessa
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // HTTPS em produ��o
    });

// Configurar pol�ticas de autoriza��o (opcional)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("TipoUsuario", "Admin"));

    options.AddPolicy("ResponsavelOnly", policy =>
        policy.RequireClaim("TipoUsuario", "Responsavel"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// === IMPORTANTE: A ORDEM IMPORTA! ===
app.UseAuthentication();    // DEVE vir ANTES de UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Rotas customizadas para �reas espec�ficas
app.MapControllerRoute(
    name: "cadastro",
    pattern: "Cadastro",
    defaults: new { controller = "Responsavel", action = "Cadastro" });

app.MapControllerRoute(
    name: "login",
    pattern: "Login",
    defaults: new { controller = "Auth", action = "Login" });

app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=Dashboard}",
    defaults: new { controller = "Admin" });

app.Run();