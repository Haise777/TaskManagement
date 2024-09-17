using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagement.API.Data;
using TaskManagement.API.Models;

namespace TaskManagement.API.Services
{
    public class TaskService
    {
        private readonly MyDbContext _db;
        private readonly UserManager<User> _userManager;

        public TaskService(UserManager<User> userManager, MyDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<IEnumerable<ReadableTask>> GetAllAssignedTasksAsync(string userId)
        {
            var userAssignedTasks = await _db.Tasks
                .Where(t => t.UserTasks.Any(t => t.UserId == userId)).Include(inc => inc.UserTasks)
                .ToListAsync();

            return await TranslateTasksAsync(userAssignedTasks);
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
            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();

            return true;
        }

        //Should return based on the success of the operation
        public async Task<bool> ModifyTaskAsync(ClaimsPrincipal user, TaskDto modifiedTask, int taskId)
        {
            var dbTask = await _db.Tasks
                .Where(x => x.Id == taskId)
                .Include(x => x.UserTasks).SingleAsync();

            if (!CheckIfAllowed(user, dbTask.AuthorId))
                return false;

            dbTask.Title = modifiedTask.Title;
            dbTask.Description = modifiedTask.Description;
            dbTask.LastUpdated = DateTime.UtcNow;
            dbTask.UserTasks.Clear();
            foreach (var filteredUserTask in LinkUsersToTask(modifiedTask.AssignedUsers, taskId))
                dbTask.UserTasks.Add(filteredUserTask);

            await _db.SaveChangesAsync();
            return true;
        }

        //Should return based on the success of the operation
        public async Task<bool> DeleteTaskAsync(ClaimsPrincipal user, int taskId)
        {
            var dbTask = await _db.Tasks.FirstAsync(x => x.Id == taskId);

            if (!CheckIfAllowed(user, dbTask.AuthorId))
                return false;

            _db.Tasks.Remove(dbTask);
            await _db.SaveChangesAsync();
            return true;
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

        private bool CheckIfAllowed(ClaimsPrincipal user, string authorId)
        {
            if (authorId != user.FindFirst(ClaimTypes.NameIdentifier)!.Value)
                if (!user.FindAll(ClaimTypes.Role).Any(x => x.Value == "Admin"))
                    return false;

            return true;
        }

        private async Task<IEnumerable<ReadableTask>> TranslateTasksAsync(IEnumerable<MyTask> tasks)
        {
            var translatedTasks = new List<ReadableTask>();

            var userIds = tasks
                .SelectMany(t => t.UserTasks)
                .Select(t => t.UserId)
                .Distinct().ToList();
            var userNames = await _db.Users
                .Where(x => userIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, x => x.UserName);

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
    }
}
