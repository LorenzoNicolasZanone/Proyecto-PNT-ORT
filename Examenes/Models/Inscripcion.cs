
namespace Examenes.Models
{
    public class Inscripcion
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }

        public int AlumnoId { get; set; }
        public virtual Alumno Alumno { get; set; }
        public int CursoId { get; set; }
        public virtual Curso Curso { get; set; }
    }
}