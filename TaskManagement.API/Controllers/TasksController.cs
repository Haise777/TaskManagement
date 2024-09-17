using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.API.Data;
using TaskManagement.API.Models;
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

        [HttpGet("getassignedtasks")]
        public async Task<IActionResult> GetAssignedTasks()
        {
            var tasks = await _taskService.GetAllAssignedTasksAsync(
                HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            return Ok(tasks);
        }

        //TODO Not implemented yet
        [HttpPost("modifytask")]
        public async Task<IActionResult> ModifyTask([FromBody]MyTask task)
        {
            return BadRequest("Action not implemented yet");
            var result = await _taskService.ModifyTaskAsync(HttpContext.User, task);

            return Ok(result);
        }

        [HttpDelete("deletetask/{taskId}")]
        public async Task<IActionResult> DeleteTask([FromRoute]int taskId)
        {
            var result = await _taskService.DeleteTaskAsync(HttpContext.User, taskId);

            return Ok(result);
        }

        [HttpPost("createtask")]
        public async Task<IActionResult> CreateTask([FromBody]TaskDto newTask)
        {
            await _taskService.CreateNewTaskAsync(HttpContext.User,newTask);
            return Ok("Success");
        }
    }
}
