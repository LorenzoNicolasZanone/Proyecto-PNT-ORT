
namespace Examenes.Models
{
    public class Materia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double NotaDeAprobacion { get; set; }
        public double NotaDePromocion { get; set; }

        public virtual ICollection<Curso> Cursos { get; set; }
    }
}