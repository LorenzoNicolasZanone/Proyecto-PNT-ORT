using Examenes.Models;
using System.Collections.Generic;

namespace Examenes.Models.ViewModels
{
    public class MisExamenesViewModel
    {
        public string CursoNombre { get; set; }
        public int CursoId { get; set; }
        public List<ExamenAlumno> ExamenesDelAlumno { get; set; }
    }
}