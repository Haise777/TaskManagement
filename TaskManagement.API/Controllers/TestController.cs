using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("Main")]
        public IActionResult Index()
        {
            return Ok("Main page of Test");
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("Admin")]
        public IActionResult AdminPage()
        {
            return Ok("Admin page of Test");
        }

        [HttpGet("Regular")]
        public IActionResult RegularPage()
        {
            return Ok("Regular page of Test");
        }
    }
}
