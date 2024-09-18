using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.DataTransfer;
using TaskManagement.API.Services;

namespace TaskManagement.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
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
        public async Task<IActionResult> ChangeUsername([FromBody] Username newUsername)
        {
            var result = await _accountService.ChangeUsernameAsync(HttpContext.User, newUsername);
            if (result is not null)
                return BadRequest(result);

            return Ok("Username changed");
        }

        [HttpDelete("deleteAccount")]
        public async Task<IActionResult> DeleteAccount([FromBody] UserPassword userPassword)
        {
            var result = await _accountService.DeleteUserAccountAsync(HttpContext.User, userPassword.Password);
            if (result is not null)
                return BadRequest(result);

            return Ok("Username changed");
        }


        // Admin-section //

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("admin/changeUsername/{userId:guid}")]
        public async Task<IActionResult> AdminChangeUsername([FromRoute] string userId, [FromBody] Username newUsername)
        {
            var result = await _accountService.AdminChangeUsernameAsync(userId, newUsername);
            if (result is not null)
                return BadRequest(result);

            return Ok("Username changed");
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("admin/deleteAccount/{userId:guid}")]
        public async Task<IActionResult> AdminDeleteAccount([FromRoute] string userId)
        {
            var result = await _accountService.AdminDeleteUserAccountAsync(userId);
            if (result is not null)
                return BadRequest(result);

            return Ok("Username changed");
        }
    }
}
