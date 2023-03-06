using DemandaDiaria.Enums;
using System.ComponentModel.DataAnnotations;

namespace DemandaDiaria.Data.Entities
{
    public class Demanda
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Date { get; set; }

        public User User { get; set; }

        [Display(Name = "Uni")]
        public string UniUser { get; set; }

        [Display(Name = "Status")]
        public DemandaStatus DemandaStatus { get; set; }

        public ICollection<Product> Products { get; set; }

        [Display(Name = "Productos")]
        public int ProductsNumber => Products == null ? 0 : Products.Count;

    }
}
