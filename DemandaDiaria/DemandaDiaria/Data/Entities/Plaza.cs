using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DemandaDiaria.Data.Entities
{
    public class Plaza
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Plaza")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        public ICollection<Sucursal> Sucursales { get; set; }

        [Display(Name = "Sucursales")]
        public int SucursalesNumber => Sucursales == null ? 0 : Sucursales.Count;


    }
}
