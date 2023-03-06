using DemandaDiaria.Data;
using DemandaDiaria.Data.Entities;
using DemandaDiaria.Enums;
using DemandaDiaria.Helpers;
using DemandaDiaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemandaDiaria.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelpers _userHelper;
        private readonly ICombosHelper _combosHelper;

        public UsersController(DataContext context, IUserHelpers userHelper, ICombosHelper combosHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Users
                .Include(u => u.Sucursal)
                .ThenInclude(s => s.Plaza)
                .ToListAsync());
        }

        public async Task<IActionResult> Create()
        {
            AddUserViewModel model = new AddUserViewModel
            {
                Id = Guid.Empty.ToString(),
                Plazas = await _combosHelper.GetComboPlazasAsync(),
                Sucursales = await _combosHelper.GetComboSucursalesAsync(0),
                UserType = UserType.Admin,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.AddUserAsync(model);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                    model.Plazas = await _combosHelper.GetComboPlazasAsync();
                    model.Sucursales = await _combosHelper.GetComboSucursalesAsync(model.PlazaId);
                    return View(model);
                }             
                    return RedirectToAction("Index", "Home");               
            }

            model.Plazas = await _combosHelper.GetComboPlazasAsync();
            model.Sucursales = await _combosHelper.GetComboSucursalesAsync(model.PlazaId);
            return View(model);
        }

        public JsonResult GetSucursales(int plazaId)
        {
            Plaza plaza = _context.Plazas
                .Include(p => p.Sucursales)
                .FirstOrDefault(c => c.Id == plazaId);
            if (plaza == null)
            {
                return null;
            }

            return Json(plaza.Sucursales.OrderBy(d => d.Name));
        }
    }
}
