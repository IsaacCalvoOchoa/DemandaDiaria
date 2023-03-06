using Microsoft.AspNetCore.Mvc.Rendering;

namespace DemandaDiaria.Helpers
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboPlazasAsync();

        Task<IEnumerable<SelectListItem>> GetComboSucursalesAsync(int plazaId);

    }
}
