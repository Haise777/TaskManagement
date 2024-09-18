using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.DataTransfer
{
    public class UserPassword
    {
        [Required]
        public string Password { get; set; }
    }
}
