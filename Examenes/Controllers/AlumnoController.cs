using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Examenes.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims; 

namespace Examenes.Controllers
{
     
    [Authorize(Roles = "ALUMNO")]
    public class AlumnoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AlumnoController> _logger;

        public AlumnoController(ApplicationDbContext context, ILogger<AlumnoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var usuarioIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(usuarioIdString))
            {
                _logger.LogWarning("No se pudo encontrar el Claim NameIdentifier para el usuario.");
                return RedirectToAction("Logout", "Home");
            }

            var usuarioId = int.Parse(usuarioIdString);

            var alumno = await _context.Alumnos
                                   .FirstOrDefaultAsync(a => a.UsuarioId == usuarioId);

            if (alumno == null)
            {
                _logger.LogError("No se encontró un perfil de Alumno para el UsuarioId: {UsuarioId}", usuarioId);
                return NotFound("No se encontró un perfil de alumno asociado a tu usuario.");
            }

            var inscripciones = await _context.Inscripciones
                .Where(i => i.AlumnoId == alumno.Id)
                .Include(i => i.Curso)
                    .ThenInclude(c => c.Materia)
                .Include(i => i.Curso)
                    .ThenInclude(c => c.Profesor)
                        .ThenInclude(p => p.Usuario)
                .OrderBy(i => i.Curso.Nombre)
                .ToListAsync();

            return View(inscripciones);
        }
    }
}