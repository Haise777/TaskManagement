using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Data.DataTransfer
{
    public class UserPassword
    {
        [Required]
        public string Password { get; set; }
    }
}
