using TaskManagement.API.Data.Models;

namespace TaskManagement.API.Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserAsync(string userId);
    }
}
