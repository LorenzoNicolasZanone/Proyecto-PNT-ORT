using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Examenes.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Examenes.Models.ViewModels;
using Examenes.Models; 

namespace Examenes.Controllers
{
    [Authorize(Roles = "PROFESOR")]
    public class RespuestasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RespuestasController> _logger;

        public RespuestasController(ApplicationDbContext context, ILogger<RespuestasController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> Index(int examenId)
        {
            var examen = await _context.Examenes
                .Include(e => e.Curso)
                .FirstOrDefaultAsync(e => e.Id == examenId);

            if (examen == null) return NotFound();

            var alumnosInscriptos = await _context.Inscripciones
                .Where(i => i.CursoId == examen.CursoId)
                .Include(i => i.Alumno) 
                    .ThenInclude(a => a.Usuario)
                .Select(i => i.Alumno)
                .ToListAsync();

            var resultadosExistentes = await _context.ExamenesAlumnos
                .Where(ea => ea.ExamenId == examenId)
                .ToListAsync();

            var listaDeResultados = new List<ExamenAlumno>();

            foreach (var alumno in alumnosInscriptos)
            {
                var resultado = resultadosExistentes.FirstOrDefault(r => r.AlumnoId == alumno.Id);

                if (resultado != null)
                {
                    resultado.Alumno = alumno; 
                    listaDeResultados.Add(resultado);
                }
                else
                {
                    listaDeResultados.Add(new ExamenAlumno
                    {
                        AlumnoId = alumno.Id,
                        Alumno = alumno, 
                        ExamenId = examenId,
                        Estado = examen.Fin > DateTime.Now ? EstadoExamen.PENDIENTE : EstadoExamen.AUSENTE,
                        Nota = null,
                        CantRespCorrectas = null
                    });
                }
            }
            
            var viewModel = new RespuestasViewModel
            {
                ExamenId = examen.Id,
                ExamenTitulo = examen.Titulo,
                CursoNombre = examen.Curso.Nombre,
                CursoId = examen.CursoId,
                ResultadosAlumnos = listaDeResultados
                                        .OrderBy(r => r.Alumno.Usuario.Nombre)
                                        .ToList()
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "PROFESOR")]
        public async Task<IActionResult> DetalleRespuesta(int examenAlumnoId)
        {
            var examenAlumno = await _context.ExamenesAlumnos
                .Include(ea => ea.Alumno.Usuario)
                .Include(ea => ea.Examen.Curso)
                .Include(ea => ea.Respuestas)
                    .ThenInclude(r => r.Opcion)
                .FirstOrDefaultAsync(ea => ea.Id == examenAlumnoId);

            if (examenAlumno == null) return NotFound();

            var todasLasPreguntas = await _context.Preguntas
                .Where(p => p.ExamenId == examenAlumno.ExamenId)
                .Include(p => p.Opciones)
                .ToListAsync();

            var preguntasCompletas = new List<PreguntaConRespuesta>();

            foreach (var pregunta in todasLasPreguntas)
            {
                var respuestaDelAlumno = examenAlumno.Respuestas
                    .FirstOrDefault(r => r.Opcion.PreguntaId == pregunta.Id);

                preguntasCompletas.Add(new PreguntaConRespuesta
                {
                    Pregunta = pregunta,
                    OpcionSeleccionadaId = respuestaDelAlumno?.OpcionId,
                    EsCorrecta = respuestaDelAlumno?.Opcion.EsCorrecta ?? false
                });
            }

            var viewModel = new RespuestasDetalleViewModel
            {
                ExamenAlumno = examenAlumno,
                CursoId = examenAlumno.Examen.Curso.Id,
                PreguntasCompletas = preguntasCompletas
            };

            return View("DetalleRespuesta", viewModel);
        }
    }
}