using System.ComponentModel.DataAnnotations;

namespace DemandaDiaria.Models
{
    public class SucursalViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Sucursal")]
        [MaxLength(50, ErrorMessage = "El campo {0}, debe de contener maximo {1} caracteres.")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        public int PlazaId { get; set; }
    }
}
