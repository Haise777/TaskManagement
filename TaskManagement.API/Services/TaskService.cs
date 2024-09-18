using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagement.API.Contracts;
using TaskManagement.API.Data.DataTransfer;
using TaskManagement.API.Data.Models;

namespace TaskManagement.API.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _repo;
        private readonly UserManager<User> _userManager;

        public TaskService(UserManager<User> userManager, ITaskRepository repo)
        {
            _userManager = userManager;
            _repo = repo;
        }

        public async Task<IEnumerable<ReadableTask>> GetAllAssignedTasksAsync(string userId)
        {
            var userAssignedTasks = await _repo.GetAllAssignedTasksAsync(userId);

            return await TranslateTasksAsync(userAssignedTasks);
        }
        public async Task<IEnumerable<ReadableTask>> GetAllAuthorTasksAsync(string authorId)
        {
            var authorTasks = await _repo.GetAllAuthorTasksAsync(authorId);

            return await TranslateTasksAsync(authorTasks);
        }

        //Should return based on the success of the operation
        public async Task<bool> CreateNewTaskAsync(ClaimsPrincipal user, TaskDto newTask)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var task = new MyTask()
            {
                AuthorId = userId,
                Title = newTask.Title,
                Description = newTask.Description,
                Created = DateTime.UtcNow,
            };


            task.UserTasks = LinkUsersToTask(newTask.AssignedUsers, task);

            //TODO implement a try catch error handling in the future
            await _repo.SaveDataAsync(task);

            return true;
        }

        //Should return based on the success of the operation
        public async Task<bool> ModifyTaskAsync(ClaimsPrincipal user, TaskDto modifiedTask, int taskId, bool admin = false)
        {
            var dbTask = await _repo.GetTaskAsync(taskId, IncludeUserTask: true);

            if (!CheckIfAuthor(user, dbTask.AuthorId))
                if (!admin) return false;

            dbTask.Title = modifiedTask.Title;
            dbTask.Description = modifiedTask.Description;
            dbTask.LastUpdated = DateTime.UtcNow;
            dbTask.UserTasks.Clear();
            foreach (var filteredUserTask in LinkUsersToTask(modifiedTask.AssignedUsers, taskId))
                dbTask.UserTasks.Add(filteredUserTask);

            await _repo.UpdateModelAsync(dbTask);
            return true;
        }

        //Should return based on the success of the operation
        public async Task<bool> DeleteTaskAsync(ClaimsPrincipal user, int taskId, bool admin = false)
        {
            var dbTask = await _repo.GetTaskAsync(taskId);

            if (!CheckIfAuthor(user, dbTask.AuthorId))
                if (!admin) return false;

            await _repo.RemoveTaskAsync(dbTask);
            return true;
        }

        private async Task<IEnumerable<ReadableTask>> TranslateTasksAsync(IEnumerable<MyTask> tasks)
        {
            var translatedTasks = new List<ReadableTask>();

            var userIds = tasks
                .SelectMany(t => t.UserTasks)
                .Select(t => t.UserId)
                .Distinct().ToList();
            var userNames = await _repo.GetUsernamesById(userIds);

            foreach (var task in tasks)
            {
                translatedTasks.Add(new ReadableTask
                {
                    Id = task.Id,
                    Author = userNames.Single(x => x.Key == task.AuthorId),
                    Title = task.Title,
                    Description = task.Description,
                    Created = task.Created,
                    LastUpdated = task.LastUpdated,
                    AssignedUsers = userNames
                    .Where(un => task.UserTasks.Any(ut => ut.UserId == un.Key))
                    .ToDictionary(x => x.Key, x => x.Value)
                });
            }

            return translatedTasks;
        }

        private ICollection<UserTask> LinkUsersToTask(IEnumerable<string>? assignedUsersId, MyTask myTask)
        {
            var userTasks = new List<UserTask>();

            if (assignedUsersId != null)
                foreach (var userId in assignedUsersId)
                {
                    userTasks.Add(new UserTask
                    {
                        Task = myTask,
                        UserId = userId
                    });
                }

            return userTasks;
        }

        private ICollection<UserTask> LinkUsersToTask(IEnumerable<string>? assignedUsersId, int taskId)
        {
            var userTasks = new List<UserTask>();

            if (assignedUsersId != null)
                foreach (var userId in assignedUsersId)
                {
                    userTasks.Add(new UserTask
                    {
                        TaskId = taskId,
                        UserId = userId
                    });
                }

            return userTasks;
        }

        private bool CheckIfAuthor(ClaimsPrincipal user, string authorId)
        {
            if (authorId != user.FindFirst(ClaimTypes.NameIdentifier)!.Value)
                return false;

            return true;
        }
    }
}
