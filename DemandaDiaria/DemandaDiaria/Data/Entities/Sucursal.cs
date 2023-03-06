using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemandaDiaria.Data.Entities
{
    public class Sucursal
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Sucursal")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Plaza Plaza { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
