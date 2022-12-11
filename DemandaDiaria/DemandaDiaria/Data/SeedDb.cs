using DemandaDiaria.Data.Entities;
using DemandaDiaria.Eums;
using DemandaDiaria.Helpers;
using System.Net;

namespace DemandaDiaria.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            //await CheckSucursalesAsync();
            await CheckPlazasAsync();
            await CheckRolesAsync();
            await CheckUserAsync("00106968", "Isaac", "Calvo", "isaaccalvoochoa@gmail.com", UserType.Admin);
        }

        private async Task<User> CheckUserAsync(
    string uni,
    string firstName,
    string lastName,
    string email,
    UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    Uni = uni,
                    Sucursal = _context.Sucursales.FirstOrDefault(),
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "00106968");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Roc.ToString());
            await _userHelper.CheckRoleAsync(UserType.Rf.ToString());
            await _userHelper.CheckRoleAsync(UserType.Adi.ToString());
        }


        private async Task CheckPlazasAsync()
        {
            if (!_context.Plazas.Any())
            {
                _context.Plazas.Add(new Plaza
                {
                    Name = "Mexicali",
                    Sucursales = new List<Sucursal>()
                    {
                        new Sucursal { Name = "Anahuac" },
                        new Sucursal { Name = "Xochimilco" },
                        new Sucursal { Name = "Madero B" },
                    },
                });
                _context.Plazas.Add(new Plaza
                {
                    Name = "Tijuana",
                    Sucursales = new List<Sucursal>()
                    {
                        new Sucursal { Name = "Gobernador Lugo" },
                        new Sucursal { Name = "Ermita" },
                        new Sucursal { Name = "Castro" },
                    },
                });
            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckSucursalesAsync()
        {
            if (!_context.Sucursales.Any())
            {
                _context.Sucursales.Add(new Sucursal { Name = "Anahuac" });
                _context.Sucursales.Add(new Sucursal { Name = "Xochimilco" });
                _context.Sucursales.Add(new Sucursal { Name = "Madero B" });
                _context.Sucursales.Add(new Sucursal { Name = "Galerias" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task<User> CheckUserAsync(string uni, string firstName, string lastName, UserType userType, Sucursal sucursal)
        {
            User user = await _userHelper.GetUserAsync(uni);
            if (user == null)
            {
                user = new User
                {
                    Uni = uni,
                    FirstName = firstName,
                    LastName = lastName,
                    UserType = userType,
                    Sucursal = _context.Sucursales.FirstOrDefault(),
                };

                await _userHelper.AddUserAsync(user, "00106968");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;

        }

        //private async Task CheckRolesAsync()
        //{
        //    await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
        //    await _userHelper.CheckRoleAsync(UserType.Roc.ToString());
        //    await _userHelper.CheckRoleAsync(UserType.Rf.ToString());
        //    await _userHelper.CheckRoleAsync(UserType.Adi.ToString());
        //}

    }
}
