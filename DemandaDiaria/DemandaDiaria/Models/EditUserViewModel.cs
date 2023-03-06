using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DemandaDiaria.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "UNI")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Uni { get; set; }

        [Display(Name = "Nombre")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string LastName { get; set; }

        [Display(Name = "Plaza")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes de seleccionar una plaza.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int PlazaId { get; set; }

        public IEnumerable<SelectListItem> Plazas { get; set; }

        [Display(Name = "Sucursal")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes de seleccionar una Sucursal.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int SucursalId { get; set; }

        public IEnumerable<SelectListItem> Sucursales { get; set; }

    }
}
