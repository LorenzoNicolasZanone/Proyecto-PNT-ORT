using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Examenes.Data;
using Microsoft.EntityFrameworkCore;
using Examenes.Models.ViewModels;
using Examenes.Models;
using System.Security.Claims; 

namespace Examenes.Controllers
{
    [Authorize]
    public class ExamenController : Controller
    {

        private readonly ILogger<ExamenController> _logger;
        private readonly ApplicationDbContext _context;

        public ExamenController(ApplicationDbContext context, ILogger<ExamenController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> ListadoPorCursoProfesor(int cursoId)
        {
            var curso = await _context.Cursos
                .Include(c => c.Materia)
                .FirstOrDefaultAsync(c => c.Id == cursoId);

            if (curso == null)
            {
                return NotFound("El curso no existe.");
            }

            var examenes = await _context.Examenes
                .Where(e => e.CursoId == cursoId && e.Publicado == true)
                .OrderBy(e => e.Inicio)
                .ToListAsync();

            var viewModel = new ExamenCursoViewModel
            {
                Examenes = examenes,
                CursoId = curso.Id,
                NombreCurso = curso.Nombre,
                NombreMateria = curso.Materia.Nombre,
                Finalizado = curso.Finalizado
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "PROFESOR")]
        public async Task<IActionResult> Crear(int cursoId)
        {
            var curso = await _context.Cursos.FindAsync(cursoId);
            if (curso == null)
            {
                return NotFound();
            }

            var viewModel = new NuevoExamenViewModel
            {
                CursoId = cursoId,
                NombreCurso = curso.Nombre
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "PROFESOR")]
        public async Task<IActionResult> Crear(NuevoExamenViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                var logDeErrores = string.Join(" | ", errores);

                _logger.LogError("El modelo no es válido. Errores: {Errores}", logDeErrores);

                _logger.LogWarning("Datos recibidos: {@ViewModel}", viewModel);

                return View(viewModel);
            }

            var nuevoExamen = new Models.Examen
            {
                Titulo = viewModel.Titulo,
                Inicio = viewModel.Inicio,
                Fin = viewModel.Fin,
                Tipo = viewModel.Tipo,
                PorcentajeParaAprobacion = viewModel.PorcentajeParaAprobacion,
                CursoId = viewModel.CursoId,
                Publicado = false
            };

            _context.Examenes.Add(nuevoExamen);
            await _context.SaveChangesAsync();

            return RedirectToAction("GestionarPreguntas", new { examenId = nuevoExamen.Id });
        }

        [HttpGet]
        [Authorize(Roles = "PROFESOR")]
        public async Task<IActionResult> GestionarPreguntas(int examenId)
        {
            var examen = await _context.Examenes
                .Include(e => e.Preguntas)
                    .ThenInclude(p => p.Opciones)
                .FirstOrDefaultAsync(e => e.Id == examenId);

            if (examen == null) return NotFound();

            var viewModel = new GestionarPreguntasViewModel
            {
                ExamenId = examen.Id,
                ExamenTitulo = examen.Titulo,
                CursoId = examen.CursoId,
                PreguntasExistentes = examen.Preguntas.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "PROFESOR")]
        public async Task<IActionResult> AgregarPregunta(GestionarPreguntasViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                  .SelectMany(v => v.Errors)
                  .Select(e => e.ErrorMessage);

                var logDeErrores = string.Join(" | ", errores);

                _logger.LogError("El modelo no es válido. Errores: {Errores}", logDeErrores);

                _logger.LogWarning("Datos recibidos: {@ViewModel}", viewModel);

                var examen = await _context.Examenes
                    .Include(e => e.Preguntas)
                        .ThenInclude(p => p.Opciones)
                    .FirstOrDefaultAsync(e => e.Id == viewModel.ExamenId);

                if (examen == null) return NotFound();

                viewModel.ExamenTitulo = examen.Titulo;
                viewModel.PreguntasExistentes = examen.Preguntas.ToList();

                return View("GestionarPreguntas", viewModel);
            }

            var nuevaPregunta = new Pregunta
            {
                Enunciado = viewModel.NuevoEnunciado,
                ExamenId = viewModel.ExamenId
            };


            var opciones = new List<Opcion>{
                new Opcion { Texto = viewModel.OpcionCorrecta, EsCorrecta = true, Pregunta = nuevaPregunta },
                new Opcion { Texto = viewModel.Opcion2, EsCorrecta = false, Pregunta = nuevaPregunta },
                new Opcion { Texto = viewModel.Opcion3, EsCorrecta = false, Pregunta = nuevaPregunta },
                new Opcion { Texto = viewModel.Opcion4, EsCorrecta = false, Pregunta = nuevaPregunta }
            };

            _context.Opciones.AddRange(opciones);
            await _context.SaveChangesAsync();

            return RedirectToAction("GestionarPreguntas", new { examenId = viewModel.ExamenId });
        }

        [HttpPost]
        [Authorize(Roles = "PROFESOR")]
        public async Task<IActionResult> PublicarExamen(int examenId, int cursoId)
        {
            var examen = await _context.Examenes
                                       .Include(e => e.Preguntas)
                                       .FirstOrDefaultAsync(e => e.Id == examenId);

            if (examen == null) return NotFound();

            if (examen.Preguntas.Count == 0)
            {
                _context.Examenes.Remove(examen);
                _logger.LogWarning("Examen {ExamenId} borrado (borrador sin preguntas).", examen.Id);
            }
            else
            {
                examen.Publicado = true;
                _logger.LogInformation("Examen {ExamenId} publicado.", examen.Id);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ListadoPorCursoProfesor", new { cursoId = cursoId });
        }

        [HttpPost]
        [Authorize(Roles = "PROFESOR")]
        public async Task<IActionResult> EliminarPregunta(int preguntaId, int examenId)
        {
            var pregunta = await _context.Preguntas.Include(p => p.Opciones).FirstOrDefaultAsync(p => p.Id == preguntaId);

            if (pregunta == null)
            {
                _logger.LogWarning("Se intentó borrar una pregunta (ID: {PreguntaId}) que no existe.", preguntaId);
                return NotFound();
            }

            if (examenId != pregunta.ExamenId)
            {
                _logger.LogError("Error de seguridad: Intento de borrar pregunta {PreguntaId} desde un examen {ExamenId} incorrecto.", preguntaId, examenId);
                return BadRequest();
            }

            _context.Preguntas.Remove(pregunta);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Pregunta {PreguntaId} eliminada.", preguntaId);

            return RedirectToAction("GestionarPreguntas", new { examenId = examenId });
        }

        [Authorize(Roles = "PROFESOR")]
        public async Task<IActionResult> Modificar(int id)
        {
            var examen = await _context.Examenes
                                       .Include(e => e.Curso)
                                       .FirstOrDefaultAsync(e => e.Id == id);

            if (examen == null) return NotFound();

            if (examen.Inicio <= DateTime.Now)
            {
                _logger.LogWarning("Intento de modificar examen {ExamenId} ya iniciado.", id);
                TempData["Error"] = "No se puede modificar un examen que ya ha comenzado.";
                return RedirectToAction("ListadoPorCursoProfesor", new { cursoId = examen.CursoId });
            }

            var viewModel = new NuevoExamenViewModel
            {
                ExamenId = examen.Id,
                CursoId = examen.CursoId,
                NombreCurso = examen.Curso.Nombre,
                Titulo = examen.Titulo,
                Tipo = examen.Tipo,
                Inicio = examen.Inicio,
                Fin = examen.Fin,
                PorcentajeParaAprobacion = examen.PorcentajeParaAprobacion
            };

            return View("Modificar", viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "PROFESOR")]
        public async Task<IActionResult> Modificar(NuevoExamenViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var examen = await _context.Examenes.FindAsync(viewModel.ExamenId);
                if (examen == null) return NotFound();

                if (examen.Inicio <= DateTime.Now)
                {
                    _logger.LogWarning("Intento de GUARDAR examen {ExamenId} ya iniciado.", viewModel.ExamenId);
                    ModelState.AddModelError("", "Este examen ya ha comenzado y no puede ser modificado.");
                    viewModel.NombreCurso = (await _context.Cursos.FindAsync(viewModel.CursoId))?.Nombre;
                    return View("Modificar", viewModel);
                }

                examen.Titulo = viewModel.Titulo;
                examen.Inicio = viewModel.Inicio;
                examen.Fin = viewModel.Fin;
                examen.Tipo = viewModel.Tipo;
                examen.PorcentajeParaAprobacion = viewModel.PorcentajeParaAprobacion;

                await _context.SaveChangesAsync();
                _logger.LogInformation("Examen {ExamenId} modificado.", examen.Id);

                TempData["Exito"] = "Examen modificado correctamente.";
                return RedirectToAction("ListadoPorCursoProfesor", new { cursoId = examen.CursoId });
            }

            viewModel.NombreCurso = (await _context.Cursos.FindAsync(viewModel.CursoId))?.Nombre;
            return View("Modificar", viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "PROFESOR")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var examen = await _context.Examenes
                                       .Include(e => e.Curso)
                                       .FirstOrDefaultAsync(e => e.Id == id);

            if (examen == null) return NotFound();

            if (examen.Inicio <= DateTime.Now)
            {
                _logger.LogWarning("Intento de eliminar examen {ExamenId} ya iniciado.", id);
                TempData["Error"] = "No se puede eliminar un examen que ya ha comenzado.";
                return RedirectToAction("ListadoPorCursoProfesor", new { cursoId = examen.CursoId });
            }

            return View("Eliminar", examen);
        }

        [HttpPost]
        [Authorize(Roles = "PROFESOR")]
        public async Task<IActionResult> Eliminar(int examenId, int cursoId) // Recibimos los IDs del form
        {
            var examen = await _context.Examenes.FindAsync(examenId);
            if (examen == null) return NotFound();

            _context.Examenes.Remove(examen);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Examen {ExamenId} eliminado.", examenId);
            TempData["Exito"] = "Examen eliminado correctamente.";
            return RedirectToAction("ListadoPorCursoProfesor", new { cursoId = cursoId });
        }

        [HttpGet]
        [Authorize(Roles = "ALUMNO")]
        public async Task<IActionResult> ListadoPorCursoAlumno(int cursoId)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var alumno = await _context.Alumnos.FirstOrDefaultAsync(a => a.UsuarioId == usuarioId);
            if (alumno == null) return Unauthorized();

            var curso = await _context.Cursos.FindAsync(cursoId);
            if (curso == null) return NotFound();

            var examenes = await _context.ExamenesAlumnos
                .Where(ea => ea.AlumnoId == alumno.Id 
                        && ea.Examen.CursoId == cursoId 
                        && ea.Examen.Publicado)
                .Include(ea => ea.Examen)
                .OrderBy(ea => ea.Examen.Inicio)
                .ToListAsync();

            var viewModel = new MisExamenesViewModel
            {
                CursoId = cursoId,
                CursoNombre = curso.Nombre,
                ExamenesDelAlumno = examenes
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "ALUMNO")]
        public async Task<IActionResult> Rendir(int examenAlumnoId)
        {
            var now = DateTime.Now;

            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var alumno = await _context.Alumnos.FirstOrDefaultAsync(a => a.UsuarioId == usuarioId);
            if (alumno == null) return Unauthorized();

            var examenAlumno = await _context.ExamenesAlumnos
                .Include(ea => ea.Examen.Curso)
                .FirstOrDefaultAsync(ea => ea.Id == examenAlumnoId);

            if (examenAlumno == null) return NotFound();
            
            if (examenAlumno.AlumnoId != alumno.Id)
            {
                _logger.LogWarning("Intento de acceso no autorizado al examen {ExamenAlumnoId} por el alumno {AlumnoId}", examenAlumnoId, alumno.Id);
                return Unauthorized("No tienes permiso para rendir este examen.");
            }
            
            if (examenAlumno.Estado != EstadoExamen.PENDIENTE)
            {
                TempData["Error"] = "Ya has rendido este examen.";
                return RedirectToAction("ListadoPorCursoAlumno", new { cursoId = examenAlumno.Examen.CursoId });
            }

            if (now < examenAlumno.Examen.Inicio)
            {
                TempData["Error"] = "Este examen aún no ha comenzado.";
                return RedirectToAction("ListadoPorCursoAlumno", new { cursoId = examenAlumno.Examen.CursoId });
            }

            if (now > examenAlumno.Examen.Fin)
            {
                TempData["Error"] = "El tiempo para rendir este examen ya ha finalizado.";
                return RedirectToAction("ListadoPorCursoAlumno", new { cursoId = examenAlumno.Examen.CursoId });
            }
                    
            var preguntas = await _context.Preguntas
            .Where(p => p.ExamenId == examenAlumno.ExamenId)
            .Include(p => p.Opciones) // ¡Cargamos las opciones!
            .ToListAsync();

            var rnd = new Random();

            preguntas = preguntas.OrderBy(p => rnd.Next()).ToList();

            foreach (var p in preguntas)
            {
                p.Opciones = p.Opciones.OrderBy(o => rnd.Next()).ToList();
            }

            var viewModel = new RendirExamenViewModel
            {
                ExamenAlumnoId = examenAlumno.Id,
                ExamenTitulo = examenAlumno.Examen.Titulo,
                CursoNombre = examenAlumno.Examen.Curso.Nombre,
                Preguntas = preguntas,
                FechaFin = examenAlumno.Examen.Fin
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "ALUMNO")]
        public async Task<IActionResult> Entregar(int examenAlumnoId, Dictionary<int, int> respuestas)
        {
            var now = DateTime.Now;
            _logger.LogInformation("Iniciando entrega para el intento: {ExamenAlumnoId}", examenAlumnoId);

            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var alumno = await _context.Alumnos.FirstOrDefaultAsync(a => a.UsuarioId == usuarioId);
            if (alumno == null) return Unauthorized();

            var examenAlumno = await _context.ExamenesAlumnos
                .Include(ea => ea.Examen)
                .FirstOrDefaultAsync(ea => ea.Id == examenAlumnoId);

            if (examenAlumno == null) return NotFound();

            if (examenAlumno.Estado != EstadoExamen.PENDIENTE)
            {
                _logger.LogWarning("ENTREGA DUPLICADA: El examen {ExamenAlumnoId} ya fue rendido.", examenAlumnoId);
                TempData["Error"] = "Ya has entregado este examen.";
                return RedirectToAction("ListadoPorCursoAlumno", new { cursoId = examenAlumno.Examen.CursoId });
            }

            if (now > examenAlumno.Examen.Fin)
            {
                _logger.LogWarning("ENTREGA FUERA DE TÉRMINO: El examen {ExamenAlumnoId} finalizó.", examenAlumnoId);
                TempData["Error"] = "El tiempo para entregar este examen ha finalizado.";
                return RedirectToAction("ListadoPorCursoAlumno", new { cursoId = examenAlumno.Examen.CursoId });
            }
            
            var idsOpcionesSeleccionadas = respuestas.Values.ToList();

            int correctas = await _context.Opciones
                .CountAsync(o => idsOpcionesSeleccionadas.Contains(o.Id) && o.EsCorrecta);

            int totalPreguntas = await _context.Preguntas
                .CountAsync(p => p.ExamenId == examenAlumno.ExamenId);


            double nota = (totalPreguntas > 0) 
                ? (double) correctas / totalPreguntas * 100.0 
                : 0;

            _logger.LogInformation("Examen {ExamenAlumnoId} corregido. Correctas: {Correctas}/{Total}. Nota: {Nota}",
                examenAlumnoId, correctas, totalPreguntas, nota);

            examenAlumno.Estado = nota > examenAlumno.Examen.PorcentajeParaAprobacion
                ? EstadoExamen.APROBADO
                : EstadoExamen.DESAPROBADO;
                
            examenAlumno.FechaEntrega = now;
            examenAlumno.CantRespCorrectas = correctas;
            examenAlumno.Nota = Math.Round(nota, 2); 

            var nuevasRespuestas = new List<RespuestaAlumno>();
            foreach (var opcionId in idsOpcionesSeleccionadas)
            {
                nuevasRespuestas.Add(new RespuestaAlumno
                {
                    ExamenAlumnoId = examenAlumnoId,
                    OpcionId = opcionId
                });
            }
            
            _context.RespuestasAlumnos.AddRange(nuevasRespuestas);
            await _context.SaveChangesAsync();

            TempData["Exito"] = $"¡Examen entregado! Tu nota es: {examenAlumno.Nota:F2}";

            return RedirectToAction("ListadoPorCursoAlumno", new { cursoId = examenAlumno.Examen.CursoId });
        }

        [HttpGet]
        [Authorize(Roles = "ALUMNO")]
        public async Task<IActionResult> VerResultado(int examenAlumnoId)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var alumno = await _context.Alumnos.FirstOrDefaultAsync(a => a.UsuarioId == usuarioId);
            if (alumno == null) return Unauthorized();

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

            return View("VerResultado", viewModel);
        }
    }

}