using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TaskManagement.API.Data;

namespace TaskManagement.API.Services
{
    public class AccountService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountService(UserManager<IdentityUser> userManager)
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
    }
}
