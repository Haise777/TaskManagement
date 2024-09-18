using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagement.API.Contracts;
using TaskManagement.API.Data;
using TaskManagement.API.Data.Models;
using TaskManagement.API.DataTransfer;
using TaskManagement.API.Response;

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

        public async Task<IEnumerable<ReadableTask>> GetAllAssignedTasksAsync(ClaimsPrincipal user, int? taskPriority)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var userAssignedTasks = await _repo.GetAllAssignedTasksAsync(userId, taskPriority ?? -1);

            return await TranslateTasksAsync(userAssignedTasks);
        }
        public async Task<IEnumerable<ReadableTask>> GetAllAuthorTasksAsync(string authorId, int? taskPriority)
        {
            var authorTasks = await _repo.GetAllAuthorTasksAsync(authorId, taskPriority ?? -1);

            return await TranslateTasksAsync(authorTasks);
        }

        public async Task<ReadableTask> CreateNewTaskAsync(ClaimsPrincipal user, TaskDto newTask)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var task = new MyTask()
            {
                Priority = newTask.Priority,
                AuthorId = userId,
                Title = newTask.Title,
                Description = newTask.Description,
                Created = DateTime.UtcNow,
            };

            task.UserTasks = LinkUsersToTask(newTask.AssignedUsers, task);

            //TODO implement a try catch error handling in the future
            await _repo.SaveDataAsync(task);

            return (ReadableTask)await TranslateTasksAsync(new[] { task });
        }

        public async Task<ReadableTask?> ModifyTaskAsync(ClaimsPrincipal user, TaskDto modifiedTask, int taskId, bool admin = false)
        {
            var dbTask = await _repo.GetTaskAsync(taskId, IncludeUserTask: true);

            if (!CheckIfAuthor(user, dbTask!.AuthorId))
                if (!admin) return null;

            dbTask.IsCompleted = modifiedTask.IsCompleted;
            dbTask.Title = modifiedTask.Title;
            dbTask.Description = modifiedTask.Description;
            dbTask.Priority = modifiedTask.Priority;
            dbTask.LastUpdated = DateTime.UtcNow;
            dbTask.UserTasks.Clear();
            foreach (var filteredUserTask in LinkUsersToTask(modifiedTask.AssignedUsers, taskId))
                dbTask.UserTasks.Add(filteredUserTask);

            await _repo.UpdateModelAsync(dbTask);
            return (ReadableTask)await TranslateTasksAsync(new[] { dbTask });
        }

        public async Task<ErrorResponse?> DeleteTaskAsync(ClaimsPrincipal user, int taskId, bool admin = false)
        {
            var dbTask = await _repo.GetTaskAsync(taskId);

            if (dbTask == null) return new ErrorResponse() { StatusCode = 400, Message = "No Task was found with the specified ID"};
            if (!CheckIfAuthor(user, dbTask.AuthorId))
                if (!admin) return new ErrorResponse() { StatusCode = 403, Message = "User is not allowed to delete Task" }; ;

            await _repo.RemoveTaskAsync(dbTask);
            return null;
        }

        private async Task<IEnumerable<ReadableTask>> TranslateTasksAsync(IEnumerable<MyTask> tasks)
        {
            var translatedTasks = new List<ReadableTask>();

            var userIds = FetchIndividualIDs(tasks);
            var userNames = await _repo.GetUsernamesById(userIds);

            foreach (var task in tasks)
            {
                translatedTasks.Add(new ReadableTask
                {
                    Id = task.Id,
                    Priority = task.Priority,
                    Author = userNames.Single(x => x.Key == task.AuthorId),
                    Title = task.Title,
                    Description = task.Description,
                    Created = task.Created,
                    LastUpdated = task.LastUpdated,
                    IsCompleted = task.IsCompleted,
                    AssignedUsers = userNames
                    .Where(un => task.UserTasks.Any(ut => ut.UserId == un.Key))
                    .ToDictionary(x => x.Key, x => x.Value)
                });
            }

            return translatedTasks;
        }

        private static IEnumerable<string> FetchIndividualIDs(IEnumerable<MyTask> tasks)
        {
            var usernames = new List<string>();

            foreach (var task in tasks)
            {
                if (!usernames.Contains(task.AuthorId))
                    usernames.Add(task.AuthorId);

                foreach (var UserTask in task.UserTasks)
                {
                    if (!usernames.Contains(UserTask.UserId))
                        usernames.Add(UserTask.UserId);
                }
            }
            return usernames;
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
