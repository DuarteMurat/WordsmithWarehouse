using ClassLibrary.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using WordsmithWarehouse.Models;

namespace WordsmithWarehouse.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();
    }
}
