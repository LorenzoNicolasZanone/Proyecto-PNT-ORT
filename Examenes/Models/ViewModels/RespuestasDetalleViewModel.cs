using Examenes.Models;
using System.Collections.Generic;

namespace Examenes.Models.ViewModels
{

    public class PreguntaConRespuesta
    {
        public Pregunta Pregunta { get; set; } 
        public int? OpcionSeleccionadaId { get; set; } 
        public bool EsCorrecta { get; set; } 
    }

    public class RespuestasDetalleViewModel
    {
        public ExamenAlumno ExamenAlumno { get; set; }
        public int CursoId { get; set; }
        
        public List<PreguntaConRespuesta> PreguntasCompletas { get; set; }
    }
}