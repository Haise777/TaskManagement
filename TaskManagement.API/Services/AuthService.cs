using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.API.Data.DataTransfer;
using TaskManagement.API.Models;

namespace TaskManagement.API.Services
{
    public class AuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<User> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        public async Task<IdentityResult> RegisterUserAsync(UserSignUp user)
        {
            var newUser = new User
            {
                UserName = user.UserName,
                Email = user.Email
            };

            var result = await _userManager.CreateAsync(newUser, user.Password);
            if (!result.Succeeded) return result;

            result = await _userManager.AddToRoleAsync(newUser, "User");


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

        public async Task<string> GenerateJWTTokenAsync(User user)
        {
            var key = Encoding.UTF8.GetBytes(_config.GetSection("JWT:SecretKey").Value);
            var claims = await GetUserClaims(user);
            var issuer = _config.GetSection("JWT:Issuer").Value;
            var audience = _config.GetSection("JWT:Audience").Value;
            var expires = DateTime.UtcNow.AddMinutes(int.Parse(_config.GetSection("JWT:Expires").Value));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Issuer = issuer,
                Audience = audience,
                Expires = expires,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(
                tokenHandler.CreateToken(tokenDescriptor));

            return token;
        }

        private async Task<ClaimsIdentity> GetUserClaims(User user)
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
