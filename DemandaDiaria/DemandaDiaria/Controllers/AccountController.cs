using DemandaDiaria.Data;
using DemandaDiaria.Data.Entities;
using DemandaDiaria.Enums;
using DemandaDiaria.Helpers;
using DemandaDiaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace DemandaDiaria.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelpers _userHelpers;
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;

        public AccountController(IUserHelpers userHelpers, DataContext context, ICombosHelper combosHelper)
        {
            _userHelpers = userHelpers;
            _context = context;
            _combosHelper = combosHelper;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelpers.LoginAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
            }

            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            await _userHelpers.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Register()
        {
            AddUserViewModel model = new AddUserViewModel
            {
                Id = Guid.Empty.ToString(),
                Plazas = await _combosHelper.GetComboPlazasAsync(),
                Sucursales = await _combosHelper.GetComboSucursalesAsync(0),
                UserType = UserType.Rf,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {             
                User user = await _userHelpers.AddUserAsync(model);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                    model.Plazas = await _combosHelper.GetComboPlazasAsync();
                    model.Sucursales = await _combosHelper.GetComboSucursalesAsync(model.PlazaId);
                    return View(model);
                }

                LoginViewModel loginViewModel = new LoginViewModel
                {
                    Password = model.Password,
                    RememberMe = false,
                    Username = model.Username
                };

                var result2 = await _userHelpers.LoginAsync(loginViewModel);

                if (result2.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

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

        public async Task<IActionResult> ChangeUser()
        {
            User user = await _userHelpers.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            EditUserViewModel model = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Sucursales = await _combosHelper.GetComboSucursalesAsync(user.Sucursal.Plaza.Id),
                SucursalId = user.Sucursal.Id,
                Plazas = await _combosHelper.GetComboPlazasAsync(),
                PlazaId = user.Sucursal.Plaza.Id,
                Id = user.Id,
                Uni = user.Uni
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {               

                User user = await _userHelpers.GetUserAsync(User.Identity.Name);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Sucursal = await _context.Sucursales.FindAsync(model.SucursalId);
                user.Uni = model.Uni;

                await _userHelpers.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");
            }

            model.Plazas = await _combosHelper.GetComboPlazasAsync();
            model.Sucursales = await _combosHelper.GetComboSucursalesAsync(0);
            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.OldPassword == model.NewPassword)
                {
                    ModelState.AddModelError(string.Empty, "Debes ingresar una contrasena diferente");
                    return View(model);
                }

                var user = await _userHelpers.GetUserAsync(User.Identity.Name);
                if (user != null)
                {
                    var result = await _userHelpers.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario no encontrado.");
                }
            }

            return View(model);
        }

    }
}
