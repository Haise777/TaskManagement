using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Data.DataTransfer
{
    public class UserSignUp : UserLogin
    {
        [Required]
        public string Email { get; set; }
    }
}
