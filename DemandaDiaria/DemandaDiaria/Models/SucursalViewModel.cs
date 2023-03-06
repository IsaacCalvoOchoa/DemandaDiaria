using System.ComponentModel.DataAnnotations;

namespace DemandaDiaria.Models
{
    public class SucursalViewModel
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Sucursal")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        public int PlazaId { get; set; }
    }
}
