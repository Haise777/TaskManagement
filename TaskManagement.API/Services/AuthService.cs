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

        public async Task RegisterUserAsync(UserSignUp user)
        {
            var newUser = new IdentityUser
            {
                UserName = user.UserName,
                Email = user.Email
            };

            var result = await _userManager.CreateAsync(newUser, user.Password);

            //TODO Implement define users into "User" role
            throw new NotImplementedException();
            
        }
    }
}
