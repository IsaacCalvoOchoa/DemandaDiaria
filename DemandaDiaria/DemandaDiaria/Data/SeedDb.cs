using DemandaDiaria.Data.Entities;
using DemandaDiaria.Enums;
using DemandaDiaria.Helpers;
using System.Net;

namespace DemandaDiaria.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelpers _userHelper;

        public SeedDb(DataContext context, IUserHelpers userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckPlazasAsync();
            await CheckRolesAsync();
            await CheckUserAsync("00106968", "Isaac", "Calvo", "isaaccalvoochoa@gmail.com", UserType.Admin);
            await CheckUserAsync("00128140", "Jorge", "Rosales", "jorgerosales@gmail.com", UserType.Rf);
        }

        private async Task<User> CheckUserAsync(string uni, string firstName, string lastName, string email, UserType userType)
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

                await _userHelper.AddUserAsync(user, user.Uni);
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }
            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Roc.ToString());
            await _userHelper.CheckRoleAsync(UserType.Rf.ToString());
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
                        new Sucursal{ Name = "Madero B"},
                        new Sucursal{ Name = "Xochimilco" },
                    }
                   
                });

                _context.Plazas.Add(new Plaza
                {
                    Name = "Tijuana",
                    Sucursales = new List<Sucursal>()
                    {
                        new Sucursal{ Name = "Gobernador Lugo"},
                        new Sucursal{ Name = "Cochimie" },
                    }

                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
