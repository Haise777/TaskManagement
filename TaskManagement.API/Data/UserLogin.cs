using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Data
{
    public class UserLogin
    {
        [Required]
        public string UserName { get; set; }
        [Required] //TODO Validate minimum length
        [MinLength(8)]
        public string Password { get; set; }
    }
}
