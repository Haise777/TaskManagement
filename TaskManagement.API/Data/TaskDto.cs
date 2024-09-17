namespace TaskManagement.API.Data
{
    public class TaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public IEnumerable<string>? AssignedUsers { get; set; }
    }
}