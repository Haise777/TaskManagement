namespace TaskManagement.API.Data.DataTransfer
{
    public class ReadableTask
    {
        public int Id { get; set; }
        public KeyValuePair<string, string> Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? LastUpdated { get; set; }
        public Dictionary<string, string> AssignedUsers { get; set; }
    }
}
