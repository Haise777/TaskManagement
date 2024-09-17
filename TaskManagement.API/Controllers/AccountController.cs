using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Data;
using TaskManagement.API.Services;

namespace TaskManagement.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordChange newPassword)
        {
            var result = await _accountService.ChangePasswordAsync(HttpContext.User, newPassword);

            if (result is not null)
                return BadRequest(result);

            return Ok("Password change succeed");
        }

        [HttpPost("changeUsername")]
    }
}
