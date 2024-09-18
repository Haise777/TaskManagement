using System.ComponentModel.DataAnnotations;
using TaskManagement.API.Data.Enums;

namespace TaskManagement.API.DataTransfer
{
    public class TaskDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public TaskPriority Priority { get; set; }
        public bool IsCompleted { get; set; }

        public IEnumerable<string>? AssignedUsers { get; set; }
    }
}