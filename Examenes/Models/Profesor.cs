namespace Examenes.Models
{
    public class Profesor
    {
        public int Id { get; set; }
        public int? UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Curso> Cursos { get; set; }
    }

}