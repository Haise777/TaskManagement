using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Data
{
    public class UserLogin
    {
        [Required]
        public string UserName { get; set; }
        [Required] //TODO Validate minimum length
        public string Password { get; set; }
    }
}
