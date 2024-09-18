using TaskManagement.API.Data.Enums;

namespace TaskManagement.API.DataTransfer
{
    public class ReadableTask
    {
        public int Id { get; set; }
        public TaskPriority Priority { get; set; }
        public KeyValuePair<string, string> Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool IsCompleted { get; set; }
        public Dictionary<string, string> AssignedUsers { get; set; }
    }
}
