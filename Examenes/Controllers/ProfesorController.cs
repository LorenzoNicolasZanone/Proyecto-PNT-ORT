using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Examenes.Data;
using Microsoft.EntityFrameworkCore;
using Examenes.Models.ViewModels;
using Examenes.Models;

namespace Examenes.Controllers
{

    [Authorize(Roles = "PROFESOR")]
    public class ProfesorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProfesorController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "PROFESOR")]
        public async Task<IActionResult> Index()
        {

            var usuarioIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdString))
            {
                return RedirectToAction("Logout", "Home");
            }

            var usuarioId = int.Parse(usuarioIdString);

            var profesor = await _context.Profesores
                                   .FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);

            if (profesor == null)
            {
                return NotFound("No se encontró un perfil de profesor asociado a tu usuario.");
            }

            var cursos = await _context.Cursos
                .Where(c => c.ProfesorId == profesor.Id)
                .Include(c => c.Materia)
                .Include(c => c.Inscripciones)
                .OrderByDescending(c => c.Nombre)
                .ToListAsync();

            return View(cursos);
        }

        [HttpGet]
        [Authorize(Roles = "PROFESOR")]
        public async Task<IActionResult> VerAlumnos(int cursoId)
        {
            var curso = await _context.Cursos
                .Include(c => c.Materia)
                .FirstOrDefaultAsync(c => c.Id == cursoId);

            if (curso == null) return NotFound();

            var notaPromocion = curso.Materia.NotaDePromocion;
            var notaAprobacion = curso.Materia.NotaDeAprobacion;

            var inscripciones = await _context.Inscripciones
                .Where(i => i.CursoId == cursoId)
                .Include(i => i.Alumno.Usuario)
                .ToListAsync();

            var examenesDelCurso = await _context.Examenes
                .Where(e => e.CursoId == cursoId
                        && e.Publicado
                        && e.Fin <= DateTime.Now)
                .ToListAsync();

            var idsParciales = examenesDelCurso
                .Where(e => e.Tipo == TipoExamen.PARCIAL)
                .Select(e => e.Id).ToList();

            var idFinal = examenesDelCurso
                .FirstOrDefault(e => e.Tipo == TipoExamen.FINAL)?.Id;

            var todosLosResultados = await _context.ExamenesAlumnos
                .Where(ea => examenesDelCurso.Select(e => e.Id).Contains(ea.ExamenId))
                .ToListAsync();

            var listaAlumnosConNotas = new List<AlumnoConNotas>();

            foreach (var inscripcion in inscripciones)
            {
                var alumnoId = inscripcion.Alumno.Id;
                var resultadosDelAlumno = todosLosResultados
                    .Where(r => r.AlumnoId == alumnoId).ToList();

                var notasParciales = resultadosDelAlumno
                    .Where(r => idsParciales.Contains(r.ExamenId) && r.Nota != null)
                    .Select(r => r.Nota.Value).ToList();

                double? promedioParciales = notasParciales.Any() ? notasParciales.Average() : (double?)null;

                double? notaFinal = resultadosDelAlumno
                    .FirstOrDefault(r => r.ExamenId == idFinal)?.Nota;

                string estadoSituacion;

                if (curso.Finzalizado) 
                {
                    if (promedioParciales >= notaPromocion)
                    {
                        estadoSituacion = "PROMOCIONADO";
                    }
                    else if (promedioParciales >= notaAprobacion)
                    {
                        estadoSituacion = "A FINAL";
                    }
                    else
                    {
                        estadoSituacion = "RECURSA";
                    }
                }
                else
                {
                    if (promedioParciales == null)
                    {
                        estadoSituacion = "Pendiente";
                    }
                    else
                    {
                        estadoSituacion = "Cursando";
                    }
                }

                listaAlumnosConNotas.Add(new AlumnoConNotas
                {
                    Alumno = inscripcion.Alumno,
                    FechaInscripcion = inscripcion.Fecha,
                    PromedioParciales = promedioParciales,
                    NotaFinal = notaFinal,
                    EstadoSituacion = estadoSituacion 
                });
            }

            var viewModel = new VerAlumnosViewModel
            {
                CursoId = cursoId,
                CursoNombre = curso.Nombre,
                Alumnos = listaAlumnosConNotas.OrderBy(a => a.Alumno.Usuario.Nombre).ToList()
            };

            return View(viewModel);
        }
    }
}