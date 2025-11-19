using System.ComponentModel.DataAnnotations;

namespace Examenes.Models.ViewModels
{
    public class GestionarPreguntasViewModel
    {
        public int ExamenId { get; set; }
        public string? ExamenTitulo { get; set; }
        public int CursoId { get; set; }

        public List<Pregunta>? PreguntasExistentes { get; set; }

        [Required(ErrorMessage = "El enunciado es obligatorio")]
        public string NuevoEnunciado { get; set; }

        [Required(ErrorMessage = "La opción correcta es obligatoria")]
        public string OpcionCorrecta { get; set; }

        [Required(ErrorMessage = "Opción 2 es obligatoria")]
        public string Opcion2 { get; set; }

        [Required(ErrorMessage = "Opción 3 es obligatoria")]
        public string Opcion3 { get; set; }

        [Required(ErrorMessage = "Opción 4 es obligatoria")]
        public string Opcion4 { get; set; }
    }
}