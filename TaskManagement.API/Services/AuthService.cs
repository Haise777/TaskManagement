using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

        public async Task<IdentityUser?> ValidateUserLoginAsync(UserLogin user)
        {
            var identityUser = await _userManager.FindByNameAsync(user.UserName);
            if (identityUser == null) return null;

            if (!await _userManager.CheckPasswordAsync(identityUser, user.Password))
                return null;

            return identityUser;
        }

        public async Task<string> GenerateJWTTokenAsync(IdentityUser user)
        {
            var key = Encoding.UTF8.GetBytes("secret");
            var claims = await GetUserClaims(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Issuer = "",
                Audience = "",
                Expires = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(
                tokenHandler.CreateToken(tokenDescriptor));

            throw new NotImplementedException();
            return token;
        }

        private async Task<ClaimsIdentity> GetUserClaims(IdentityUser user)
        {
            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
            });

            foreach (var role in await _userManager.GetRolesAsync(user))
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
