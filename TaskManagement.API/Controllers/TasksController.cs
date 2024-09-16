using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.API.Services;

namespace TaskManagement.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAssignedTasks()
        {
            var tasks = await _taskService.GetAllAssignedTasksAsync(
                HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            return Ok(tasks);
        }
    }
}
