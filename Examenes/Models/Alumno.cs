namespace Examenes.Models
{

    public class Alumno
    {
        public int Id { get; set; }

        public int? UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<Inscripcion> Inscripciones { get; set; }

        public virtual ICollection<ExamenAlumno> ExamenesAlumnos { get; set; }
    }

    
}