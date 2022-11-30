using System.ComponentModel.DataAnnotations;

namespace DemandaDiaria.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Uni")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MinLength(8, ErrorMessage = "El campo {0} debe tener {1} carácteres.")]
        public string Password { get; set; }

        [Display(Name = "Recordarme en este navegador")]
        public bool RememberMe { get; set; }

    }
}
