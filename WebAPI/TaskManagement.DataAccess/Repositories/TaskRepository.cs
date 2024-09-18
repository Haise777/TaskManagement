using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Contracts;
using TaskManagement.API.Data.Models;

namespace TaskManagement.API.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly MyDbContext _db;

        public TaskRepository(MyDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<MyTask>> GetAllAssignedTasksAsync(string userId, int priority = -1)
        {
            if (priority < 0)
                return await _db.Tasks
                    .Where(t => t.UserTasks.Any(t => t.UserId == userId)).Include(inc => inc.UserTasks)
                    .ToListAsync();
            else
                return await _db.Tasks
                    .Where(t => ((int)t.Priority) == priority)
                    .Where(t => t.UserTasks.Any(t => t.UserId == userId)).Include(inc => inc.UserTasks)
                    .ToListAsync();
        }

        public async Task<IEnumerable<MyTask>> GetAllAuthorTasksAsync(string authorId, int priority = -1)
        {
            if (priority < 0)
                return await _db.Tasks
                    .Where(t => t.AuthorId == authorId).Include(inc => inc.UserTasks).ToListAsync();
            else
                return await _db.Tasks
                    .Where(t => ((int)t.Priority) == priority)
                    .Where(t => t.AuthorId == authorId).Include(inc => inc.UserTasks).ToListAsync();
        }

        public async Task<MyTask?> GetTaskAsync(int taskId, bool IncludeUserTask = false)
        {
            if (IncludeUserTask)
            {
                return await _db.Tasks
                    .Where(x => x.Id == taskId)
                    .Include(x => x.UserTasks).SingleOrDefaultAsync();
            }
            else
                return await _db.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);
        }

        public async Task<Dictionary<string, string>> GetUsernamesById(IEnumerable<string> userIds)
        {
            return await _db.Users
                 .Where(x => userIds.Contains(x.Id))
                 .ToDictionaryAsync(x => x.Id, x => x.UserName);
        }

        public async Task RemoveTaskAsync(MyTask dbTask)
        {
            _db.Tasks.Remove(dbTask);
            await _db.SaveChangesAsync();
        }

        public async Task SaveDataAsync(MyTask task)
        {
            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateModelAsync(MyTask dbTask)
        {
            await _db.SaveChangesAsync();
        }
    }
}
