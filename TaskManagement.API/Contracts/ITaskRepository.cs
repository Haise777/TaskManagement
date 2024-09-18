using TaskManagement.API.Data.Models;

namespace TaskManagement.API.Contracts
{
    public interface ITaskRepository
    {
        Task<IEnumerable<MyTask>> GetAllAssignedTasksAsync(string userId, int priority = -1);
        Task<IEnumerable<MyTask>> GetAllAuthorTasksAsync(string authorId, int priority = -1);
        Task<MyTask?> GetTaskAsync(int taskId, bool IncludeUserTask = false);
        Task<Dictionary<string,string>> GetUsernamesById(List<string> userIds);
        Task RemoveTaskAsync(MyTask dbTask);
        Task SaveDataAsync(MyTask task);
        Task UpdateModelAsync(MyTask dbTask);
    }
}
