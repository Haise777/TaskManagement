using Microsoft.AspNetCore.Identity;
using TaskManagement.API.Data;

namespace TaskManagement.API.Services
{
    public class AuthService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AuthService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegisterUserAsync(UserSignUp user)
        {
            var newUser = new IdentityUser
            {
                UserName = user.UserName,
                Email = user.Email
            };

            var result = await _userManager.CreateAsync(newUser, user.Password);
            if (!result.Succeeded) return result;

            // Can I bond both together in a single transaction?
            result = await _userManager.AddToRoleAsync(newUser, "Admin");

            return result;
        }

        public async Task LoginAsync(UserLogin user)
        {

        }
    }
}
