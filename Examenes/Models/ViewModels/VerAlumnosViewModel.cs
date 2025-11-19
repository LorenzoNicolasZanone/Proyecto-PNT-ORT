using Examenes.Models;
using System.Collections.Generic;

namespace Examenes.Models.ViewModels
{

    public class AlumnoConNotas
    {
        public Alumno Alumno { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public double? PromedioParciales { get; set; }
        public double? NotaFinal { get; set; }
    }

    public class VerAlumnosViewModel
    {
        public string CursoNombre { get; set; }
        public int CursoId { get; set; } 
        

        public List<AlumnoConNotas> Alumnos { get; set; }
    }
}