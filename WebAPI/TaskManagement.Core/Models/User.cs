using Microsoft.AspNetCore.Identity;

namespace TaskManagement.API.Data.Models
{
    public class User : IdentityUser
    {
        public ICollection<UserTask> UserTasks { get; set; }
    }
}
