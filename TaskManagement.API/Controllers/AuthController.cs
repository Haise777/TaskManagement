using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Data;
using TaskManagement.API.Services;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> AttemptSignUp([FromBody] UserSignUp userSignUp)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterUserAsync(userSignUp);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var user = await _authService.ValidateUserLoginAsync(userSignUp);
            var token = await _authService.GenerateJWTTokenAsync(user);

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> AttemptLogin([FromBody]UserLogin userLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _authService.ValidateUserLoginAsync(userLogin);
            if (user == null)
                return BadRequest("Invalid credentials Placeholder Error");

            var token = await _authService.GenerateJWTTokenAsync(user);
            return Ok(token);
        }
    }
}
