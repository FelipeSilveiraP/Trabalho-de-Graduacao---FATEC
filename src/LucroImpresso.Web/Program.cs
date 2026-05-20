using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LucroImpresso.Web.Components;
using LucroImpresso.Web.Data;
using LucroImpresso.Web.Interfaces;
using LucroImpresso.Web.Repositories;
using LucroImpresso.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// 0. AppDbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 31)); // Ajuste para a versão exata do seu MySQL/MariaDB no WAMP

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, serverVersion));
// 1. Injeção de Dependência
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<IOrcamentoRepository, OrcamentoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IMaquinaRepository, MaquinaRepository>();
builder.Services.AddScoped<IResumoFinanceiroRepository, ResumoFinanceiroRepository>();
builder.Services.AddHostedService<DatabaseCleanupService>();

// 2. Autenticação via Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromDays(1);

        // CORREÇÃO CRÍTICA: Impede que o cookie auth tente redirecionar (302)
        // requests do hub SignalR (/_blazor). WebSocket não suporta 302 e
        // isso causava travamento do circuit e loop de reconexão.
        options.Events.OnRedirectToLogin = context =>
        {
            if (context.Request.Path.StartsWithSegments("/_blazor"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            }
            context.Response.Redirect(context.RedirectUri);
            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseHttpsRedirection();
}

// ============================================================
// CORREÇÃO CRÍTICA DE SEGURANÇA:
// UseAuthentication e UseAuthorization DEVEM vir antes do
// MapRazorComponents. Sem eles, o [Authorize] é ignorado
// e qualquer pessoa acessa as páginas sem estar logada.
// ============================================================
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// 3. Minimal API: Processa o Login
app.MapPost("/auth/login", async (HttpContext ctx, [FromForm] string usuario, [FromForm] string senha, IUsuarioRepository repo) =>
{
    var u = await repo.AutenticarAsync(usuario, senha);
    if (u != null)
    {
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, u.NomeUsuario) };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await ctx.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        return Results.Redirect("/dashboard");
    }
    return Results.Redirect("/login?error=true");
}).DisableAntiforgery(); // <- CORREÇÃO DE SEGURANÇA APLICADA AQUI

// 4. Minimal API: Processa o Logout
app.MapGet("/auth/logout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
});

// 5. Seeding: Garante que o usuário admin exista ao iniciar
using (var scope = app.Services.CreateScope())
{
    try 
    {
        var userRepo = scope.ServiceProvider.GetRequiredService<IUsuarioRepository>();
        await userRepo.CriarUsuarioPadraoAsync();
    }
    catch (Exception ex)
    {
        // Se o banco estiver fora ou sem as tabelas, ele loga o erro mas não derruba a aplicação.
        Console.WriteLine($"[AVISO DE INFRAESTRUTURA] Falha ao executar o Seeding do banco de dados: {ex.Message}");
    }
}

app.Run(); // Esta deve ser a ÚLTIMA linha do seu Program.cs
