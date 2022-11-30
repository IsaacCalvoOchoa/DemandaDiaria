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
            await CheckRolesAsync();
            //await CheckUserAsync("00106968", "Isaac", "Calvo", UserType.Admin, string.empty);
        }

        private Task CheckUserAsync(string v1, string v2, string v3, UserType admin)
        {
            throw new NotImplementedException();
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

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Roc.ToString());
            await _userHelper.CheckRoleAsync(UserType.Rf.ToString());
            await _userHelper.CheckRoleAsync(UserType.Adi.ToString());
        }

    }
}
