using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.DataTransfer
{
    public class UserLogin
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
