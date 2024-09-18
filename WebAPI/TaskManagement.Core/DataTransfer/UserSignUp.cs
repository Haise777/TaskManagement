using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.DataTransfer
{
    public class UserSignUp : UserLogin
    {
        [Required]
        public string Email { get; set; }
    }
}
