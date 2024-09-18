using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Data.DataTransfer
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