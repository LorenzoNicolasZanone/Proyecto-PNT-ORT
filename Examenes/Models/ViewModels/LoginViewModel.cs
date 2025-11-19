using System.ComponentModel.DataAnnotations;

namespace Examenes.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El tipo de usuario es requerido")]
        public string Type { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; }
    }
}