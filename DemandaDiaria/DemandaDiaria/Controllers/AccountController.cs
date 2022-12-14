using DemandaDiaria.Data;
using DemandaDiaria.Data.Entities;
using DemandaDiaria.Eums;
using DemandaDiaria.Helpers;
using DemandaDiaria.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Diagnostics.Metrics;

namespace DemandaDiaria.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        private readonly ICombosHelper _comboshelper;

        public AccountController(IUserHelper userHelper, DataContext context, ICombosHelper comboshelper)
        {
            _userHelper = userHelper;
            _context = context;
            _comboshelper = comboshelper;
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
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(String.Empty, "Uni o contrasena incorrectos");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Register()
        {
            AddUserViewModel model = new()
            {
                Id = Guid.Empty.ToString(),
                Plazas = await _comboshelper.GetComboPlazasAsync(),
                Sucursales = await _comboshelper.GetComboSucursalesAsync(0),
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

                User user = await _userHelper.AddUserAsync(model);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                    model.Plazas = await _comboshelper.GetComboPlazasAsync();
                    model.Sucursales = await _comboshelper.GetComboSucursalesAsync(model.PlazaId);    
                    return View(model);
                }

                LoginViewModel loginViewModel = new LoginViewModel
                {
                    Password = model.Password,
                    RememberMe = false,
                    Username = model.Username
                };

                var result2 = await _userHelper.LoginAsync(loginViewModel);

                if (result2.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            model.Plazas = await _comboshelper.GetComboPlazasAsync();
            model.Sucursales = await _comboshelper.GetComboSucursalesAsync(model.PlazaId);
            return View(model);
        }

        public JsonResult GetSucursales(int plazaId)
        {
            Plaza plaza = _context.Plazas
                .Include(p => p.Sucursales)
                .FirstOrDefault(p => p.Id == plazaId);
            if (plaza == null)
            {
                return null;
            }

            return Json(plaza.Sucursales.OrderBy(d => d.Name));
        }

        public async Task<IActionResult> ChangeUser()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            EditUserViewModel model = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Sucursales = await _comboshelper.GetComboSucursalesAsync(user.Sucursal.Plaza.Id),
                SucursalId = user.Sucursal.Id,
                Plazas = await _comboshelper.GetComboPlazasAsync(),
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
                User user = await _userHelper.GetUserAsync(User.Identity.Name);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Sucursal = await _context.Sucursales.FindAsync(model.SucursalId);
                user.Uni = model.Uni;

                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");
            }

            model.Plazas = await _comboshelper.GetComboPlazasAsync();
            model.Sucursales = await _comboshelper.GetComboSucursalesAsync(model.PlazaId);
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

                User? user = await _userHelper.GetUserAsync(User.Identity.Name);
                if (user != null)
                {
                    IdentityResult? result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
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
