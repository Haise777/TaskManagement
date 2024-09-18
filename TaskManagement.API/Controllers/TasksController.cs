using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.API.Data;
using TaskManagement.API.Data.DataTransfer;
using TaskManagement.API.Services;

namespace TaskManagement.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("getAssignedTasks/{taskPriority?}")]
        public async Task<IActionResult> GetAssignedTasks([FromRoute] int? taskPriority)
        {
            if (taskPriority > 3 || taskPriority < 0)
                return BadRequest($"{taskPriority} is not a valid TaskPriority value");

            var tasks = await _taskService.GetAllAssignedTasksAsync(HttpContext.User, taskPriority);

            return Ok(tasks);
        }

        [HttpGet("getAuthorTasks/{authorId:guid?}/{taskPriority?}")]
        [HttpGet("getAuthorTasks/{taskPriority:int?}")]
        public async Task<IActionResult> GetAuthorTasks([FromRoute] string? authorId, [FromRoute] int? taskPriority)
        {
            if (taskPriority > 3 || taskPriority < 0)
                return BadRequest($"{taskPriority} is not a valid TaskPriority value");

            if (authorId == null)
                authorId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var tasks = await _taskService.GetAllAuthorTasksAsync(authorId, taskPriority);

            return Ok(tasks);
        }

        [HttpPost("modifyTask/{taskId}")]
        public async Task<IActionResult> ModifyTask([FromBody] TaskDto modifiedTask, [FromRoute] int taskId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _taskService.ModifyTaskAsync(HttpContext.User, modifiedTask, taskId);
            return Ok(result);
        }

        [HttpDelete("deleteTask/{taskId}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int taskId)
        {
            var result = await _taskService.DeleteTaskAsync(HttpContext.User, taskId);
            return Ok(result);
        }

        [HttpPost("createtask")]
        public async Task<IActionResult> CreateTask([FromBody] TaskDto newTask)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _taskService.CreateNewTaskAsync(HttpContext.User, newTask);
            return Ok("Success");
        }


        // Admin-section //

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("admin/modifyTask/{taskId}")]
        public async Task<IActionResult> AdminModifyTask([FromBody] TaskDto modifiedTask, [FromRoute] int taskId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _taskService.ModifyTaskAsync(HttpContext.User, modifiedTask, taskId, admin: true);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("admin/deleteTask/{taskId}")]
        public async Task<IActionResult> AdminDeleteTask([FromRoute] int taskId)
        {
            var result = await _taskService.DeleteTaskAsync(HttpContext.User, taskId, admin: true);
            return Ok(result);
        }
    }
}
