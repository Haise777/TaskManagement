using Microsoft.AspNetCore.Identity;

namespace TaskManagement.API.Models
{
    public class User : IdentityUser
    {
        public ICollection<UserTask> UserTasks { get; set; }
    }
}
