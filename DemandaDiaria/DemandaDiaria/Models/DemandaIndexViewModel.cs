using DemandaDiaria.Data.Entities;
using DemandaDiaria.Enums;
using System.ComponentModel.DataAnnotations;

namespace DemandaDiaria.Models
{
    public class DemandaIndexViewModel
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Date { get; set; }

        [Display(Name = "Uni")]
        public string UniUser { get; set; }

        public ICollection<Product> Products { get; set; }

        [Display(Name = "Status")]
        public DemandaStatus DemandaStatus { get; set; }

        [Display(Name = "Id")]
        public Demanda DemandaId { get; set; }

        [Display(Name = "Productos")]
        public int ProductsNumber => Products == null ? 0 : Products.Count;
    }
}
