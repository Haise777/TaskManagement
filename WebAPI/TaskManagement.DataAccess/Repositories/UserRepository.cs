using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Contracts;
using TaskManagement.API.Data.Models;

namespace TaskManagement.API.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _db;

        public UserRepository(MyDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _db.Users.ToArrayAsync();
        }

        public async Task<User?> GetUserAsync(string userId)
        {
            return await _db.Users.FindAsync(userId);
        }
    }
}
