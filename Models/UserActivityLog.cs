using Library_Management_Sys.Models.Enums;

namespace Library_Management_Sys.Models
{
    public class UserActivityLog
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Permissions Action { get; set; } 
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public User user { get; set; }
    }
}
