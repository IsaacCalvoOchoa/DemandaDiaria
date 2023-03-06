using DemandaDiaria.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DemandaDiaria.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _context;

        public CombosHelper(DataContext dataContext)
        {
            _context = dataContext;
        }
        public async Task<IEnumerable<SelectListItem>> GetComboPlazasAsync()
        {
            List<SelectListItem> list = await _context.Plazas
                .Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Id.ToString()
                })
                .OrderBy(p => p.Text)
                .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Selecciona una Plaza]", Value = "0" });

            return list;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboSucursalesAsync(int plazaId)
        {
            List<SelectListItem> list = await _context.Sucursales
                .Where(s => s.Plaza.Id == plazaId)
               .Select(p => new SelectListItem
               {
                   Text = p.Name,
                   Value = p.Id.ToString()
               })
               .OrderBy(p => p.Text)
               .ToListAsync();

            list.Insert(0, new SelectListItem { Text = "[Selecciona una Sucursal]", Value = "0" });

            return list;
        }
    }
}
