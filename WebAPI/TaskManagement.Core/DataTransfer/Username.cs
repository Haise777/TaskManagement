using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.DataTransfer
{
    public class Username
    {
        [Required]
        public string NewUsername { get; set; }
    }
}