using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TaskManagement.API.Contracts;
using TaskManagement.API.Data.Models;
using TaskManagement.API.DataTransfer;

namespace TaskManagement.API.Services
{
    public class AccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _repo;

        public AccountService(UserManager<User> userManager, IUserRepository repo)
        {
            _userManager = userManager;
            _repo = repo;
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

        public async Task<IEnumerable<IdentityError>?> ChangeUsernameAsync(ClaimsPrincipal user, Username newUsername)
        {
            var iUser = await _userManager.GetUserAsync(user);
            var result = await _userManager.SetUserNameAsync(iUser, newUsername.NewUsername);

            if (!result.Succeeded)
                return result.Errors;

            return null;
        }

        public async Task<IEnumerable<IdentityError>?> AdminChangeUsernameAsync(string userId, Username newUsername)
        {
            var iUser = await _repo.GetUserAsync(userId);

            if (iUser == null)
                return new[] { new IdentityError() { Description = "Could not find a user with the specified ID" } };

            var result = await _userManager.SetUserNameAsync(iUser, newUsername.NewUsername);
            if (!result.Succeeded)
                return result.Errors;

            return null;
        }

        public async Task<IEnumerable<IdentityError>?> DeleteUserAccountAsync(ClaimsPrincipal user, string userPassword)
        {
            var iUser = await _userManager.GetUserAsync(user);

            //Checks user password
            var passwordCheck = await _userManager.CheckPasswordAsync(iUser, userPassword);
            if (!passwordCheck)
                return new[] { new IdentityError() { Description = "Invalid Password" } };

            var result = await _userManager.DeleteAsync(iUser);

            if (!result.Succeeded)
                return result.Errors;

            return null;
        }

        public async Task<IEnumerable<IdentityError>?> AdminDeleteUserAccountAsync(string userId)
        {
            var iUser = await _repo.GetUserAsync(userId);
            if (iUser == null)
                return new[] { new IdentityError() { Description = "Could not find a user with the specified ID" } };

            var result = await _userManager.DeleteAsync(iUser);

            if (!result.Succeeded)
                return result.Errors;

            return null;
        }
    }
}
