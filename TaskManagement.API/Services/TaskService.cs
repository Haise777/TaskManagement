using Microsoft.AspNetCore.Identity;
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

        public async Task<IEnumerable<MyTask>> GetAllAssignedTasksAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
