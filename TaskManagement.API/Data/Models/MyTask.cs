using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Data.Models
{
    public class MyTask
    {
        [Key]
        public int Id { get; set; }
        public TaskPriority Priority { get; set; }
        public bool IsCompleted { get; set; }
        public string AuthorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? LastUpdated { get; set; }
        public ICollection<UserTask> UserTasks { get; set; }

    }
}
