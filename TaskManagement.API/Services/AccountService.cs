using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TaskManagement.API.Data.DataTransfer;
using TaskManagement.API.Data.Models;

namespace TaskManagement.API.Services
{
    public class AccountService
    {
        private readonly UserManager<User> _userManager;

        public AccountService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<IdentityError>?> ChangePasswordAsync(ClaimsPrincipal user, PasswordChange passwords)
        {
            var identityUser = await _userManager.FindByIdAsync(
                user.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var result = await _userManager.ChangePasswordAsync(identityUser, passwords.Password, passwords.NewPassword);

            if (!result.Succeeded)
                return result.Errors;

            return null;
        }

        internal async Task<IEnumerable<IdentityError>?> ChangeUsernameAsync(ClaimsPrincipal user, Username newUsername)
        {
            var iUser = await _userManager.GetUserAsync(user);
            var result = await _userManager.SetUserNameAsync(iUser, newUsername.NewUsername);

            if (!result.Succeeded)
                return result.Errors;

            return null;
        }
    }
}
