using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace DemandaDiaria.Data.Entities
{
    public class Plaza
    {
        public int Id { get; set; }

        [Display(Name = "Plaza")]
        [MaxLength(50, ErrorMessage ="El campo {0}, debe de contener maximo {1} caracteres.")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }
    }
}
