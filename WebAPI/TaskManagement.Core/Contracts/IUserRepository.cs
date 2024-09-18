using TaskManagement.API.Data.Models;

namespace TaskManagement.API.Contracts
{
    public interface IUserRepository
    {
        Task<User?> GetUserAsync(string userId);
    }
}
