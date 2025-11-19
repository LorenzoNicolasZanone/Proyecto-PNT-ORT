using System.ComponentModel.DataAnnotations;

namespace Examenes.Models.ViewModels
{
    public class NuevoExamenViewModel
    {
        [Required(ErrorMessage = "El título es obligatorio")]
        public string Titulo { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [DataType(DataType.DateTime)]
        public DateTime Inicio { get; set; } = DateTime.Now; // Ponemos un valor por defecto

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [DataType(DataType.DateTime)]
        public DateTime Fin { get; set; } = DateTime.Now.AddHours(2); // Valor por defecto

        [Required(ErrorMessage = "El tipo de examen es obligatorio")]
        public TipoExamen Tipo { get; set; } // El Enum (PARCIAL / FINAL)

        [Required]
        [Range(1, 100, ErrorMessage = "El porcentaje debe estar entre 1 y 100")]
        public double PorcentajeParaAprobacion { get; set; } = 60; 

        public int ExamenId { get; set; }

        public int CursoId { get; set; } 
        public string? NombreCurso { get; set; } 
    }
}