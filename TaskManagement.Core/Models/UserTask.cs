namespace TaskManagement.API.Data.Models
{
    public class UserTask
    {
        public string UserId { get; set; }
        public User User { get; set; }


        public int TaskId { get; set; }
        public MyTask Task { get; set; }
    }
}
