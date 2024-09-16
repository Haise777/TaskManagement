using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Data;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("signup")]
        public async Task<IActionResult> AttemptSignUp([FromBody] UserSignUp userSignUp)
        {
            throw new NotImplementedException();
        }

        [HttpPost("login")]
        public async Task<IActionResult> AttemptLogin([FromBody]UserLogin userLogin)
        {
            throw new NotImplementedException();
        }
    }
}
