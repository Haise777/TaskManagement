using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Data
{
    public class PasswordChange
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}