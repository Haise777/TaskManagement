using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.API.Data.DataTransfer;
using TaskManagement.API.Data.Models;
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

        [HttpGet("getassignedtasks")]
        public async Task<IActionResult> GetAssignedTasks()
        {
            var tasks = await _taskService.GetAllAssignedTasksAsync(
                HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            return Ok(tasks);
        }

        [HttpGet("getauthortasks/{authorId:guid?}")]
        public async Task<IActionResult> GetAuthorTasks([FromRoute] string? authorId)
        {
            if (authorId == null)
                authorId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var tasks = await _taskService.GetAllAuthorTasksAsync(authorId);

            return Ok(tasks);
        }

        [HttpPost("modifytask/{taskId}")]
        public async Task<IActionResult> ModifyTask([FromBody] TaskDto modifiedTask, [FromRoute] int taskId)
        {
            var result = await _taskService.ModifyTaskAsync(HttpContext.User, modifiedTask, taskId);
            return Ok(result);
        }

        [HttpDelete("deletetask/{taskId}")]
        public async Task<IActionResult> DeleteTask([FromRoute] int taskId)
        {
            var result = await _taskService.DeleteTaskAsync(HttpContext.User, taskId);
            return Ok(result);
        }

        [HttpPost("createtask")]
        public async Task<IActionResult> CreateTask([FromBody] TaskDto newTask)
        {
            await _taskService.CreateNewTaskAsync(HttpContext.User, newTask);
            return Ok("Success");
        }


        // Admin-section //

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("admin/modifytask/{taskId}")]
        public async Task<IActionResult> AdminModifyTask([FromBody] TaskDto modifiedTask, [FromRoute] int taskId)
        {
            var result = await _taskService.ModifyTaskAsync(HttpContext.User, modifiedTask, taskId, admin:true);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("admin/deletetask/{taskId}")]
        public async Task<IActionResult> AdminDeleteTask([FromRoute] int taskId)
        {
            var result = await _taskService.DeleteTaskAsync(HttpContext.User, taskId, admin:true);
            return Ok(result);
        }
    }
}
