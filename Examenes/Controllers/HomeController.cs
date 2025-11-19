using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Examenes.Models;
using Microsoft.EntityFrameworkCore;
using Examenes.Data;
using Examenes.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace Examenes.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        _logger.LogInformation("Intento de login");
        if (!ModelState.IsValid)
        {
            _logger.LogInformation("No es valido el modelo: " + model.Type + " " + model.Username + " " + model.Password);
            return View("Index", model);
        }

        Usuario? usuarioEncontrado = null;

        if (model.Type == "PROFESOR")
        {
            _logger.LogInformation("Es profesor");
            var profesor = await _context.Profesores
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Usuario.Username == model.Username);

            if (profesor != null && profesor.Usuario.Password == model.Password)
            {
                usuarioEncontrado = profesor.Usuario;
            }
        }
        else
        {
            _logger.LogInformation("Es alumno");
            var alumno = await _context.Alumnos
                .Include(a => a.Usuario) // ¡Carga el Usuario relacionado!
                .FirstOrDefaultAsync(a => a.Usuario.Username == model.Username);

            if (alumno != null && alumno.Usuario.Password == model.Password)
            {
                usuarioEncontrado = alumno.Usuario;
            }
        }

        if (usuarioEncontrado != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuarioEncontrado.Username),
                new Claim(ClaimTypes.NameIdentifier, usuarioEncontrado.Id.ToString()),
                new Claim(ClaimTypes.Role, model.Type)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            );

            return RedirectToAction("Index", model.Type == "PROFESOR" ? "Profesor" : "Alumno");
        }
        else
        {
            _logger.LogInformation("No encontrado");
            ViewData["Error"] = "Usuario o contraseña incorrectos.";
            return View("Index", model);
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        _logger.LogInformation("Usuario cerró sesión.");
        return RedirectToAction("Index", "Home");
    }

}
