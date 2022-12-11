using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemandaDiaria.Data.Entities
{
    [Authorize(Roles = "Admin")]
    public class Sucursal
    {
        public int Id { get; set; }

        [Display(Name = "Sucursal")]
        [MaxLength(50, ErrorMessage = "El campo {0}, debe de contener maximo {1} caracteres.")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Name { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public Plaza Plaza { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
