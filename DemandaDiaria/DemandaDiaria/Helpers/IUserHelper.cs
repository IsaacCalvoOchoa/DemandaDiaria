using DemandaDiaria.Data.Entities;
using DemandaDiaria.Models;
using Microsoft.AspNetCore.Identity;

namespace DemandaDiaria.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserAsync(string Uni);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, string roleName);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();


    }
}
