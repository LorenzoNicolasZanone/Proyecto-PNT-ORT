using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Examenes.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "Examenes.Auth";
        options.LoginPath = "/Home/Index";
        options.AccessDeniedPath = "/Home/Index";
    });
builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<ApplicationDbContext>();

    try
    {
        await context.Database.EnsureCreatedAsync();

        if (!context.Usuarios.Any())
        {
            logger.LogInformation("La base de datos está vacía. Ejecutando script de seeding...");
            string sqlScript = await File.ReadAllTextAsync("Data/Scripts/InitialSeed.sql");
            await context.Database.ExecuteSqlRawAsync(sqlScript);
            logger.LogInformation("Script de seeding ejecutado exitosamente.");
        }
        else
        {
            logger.LogInformation("La base de datos ya contiene datos. No se ejecutó el seeding.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Ocurrió un error durante el seeding de la base de datos.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
