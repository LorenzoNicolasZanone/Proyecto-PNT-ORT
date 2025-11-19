using System.ComponentModel.DataAnnotations;

namespace Examenes.Models.ViewModels
{
    public class RespuestasViewModel
    {
        public int ExamenId { get; set; }
        public string ExamenTitulo { get; set; }
        public string CursoNombre { get; set; }
        public int CursoId { get; set; }

        public List<ExamenAlumno> ResultadosAlumnos { get; set; }
    }

}