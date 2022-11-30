using Microsoft.AspNetCore.Authorization;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace DemandaDiaria.Data.Entities
{
    [Authorize(Roles = "Admin")]
    public class Plaza
    {
        public int Id { get; set; }

        [Display(Name = "Plaza")]
        [MaxLength(50, ErrorMessage ="El campo {0}, debe de contener maximo {1} caracteres.")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        public ICollection<Sucursal> Sucursales { get; set; }
        [Display(Name ="Sucursales")]
        public int SucursalesNumber => Sucursales == null ? 0 : Sucursales.Count;
    }
}
