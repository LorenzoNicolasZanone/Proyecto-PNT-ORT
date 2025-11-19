using System.ComponentModel.DataAnnotations;

namespace Examenes.Models
{

    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        public required string Username { get; set; }
        [Required]
        public required string Password { get; set; }
        public required string Nombre { get; set; }

        public string Legajo { get; set; }
    }

}