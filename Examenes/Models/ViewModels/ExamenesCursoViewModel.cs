namespace Examenes.Models.ViewModels
{
    public class ExamenCursoViewModel
    {
        public List<Examen> Examenes { get; set; }
        public int CursoId { get; set; }
        public string NombreCurso { get; set; }
        public string NombreMateria { get; set; }
    }
}