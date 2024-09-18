using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Data.DataTransfer
{
    public class Username
    {
        [Required]
        public string NewUsername { get; set; }
    }
}