using Examenes.Models;
using System.Collections.Generic;

namespace Examenes.Models.ViewModels
{
    public class RendirExamenViewModel
    {
        public string ExamenTitulo { get; set; }
        public string CursoNombre { get; set; }
        public int ExamenAlumnoId { get; set; }
        public List<Pregunta> Preguntas { get; set; }
        public DateTime FechaFin { get; set; }
    }
}