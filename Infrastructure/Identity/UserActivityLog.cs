using Library_Management_Sys.Domain.Enums;

namespace Library_Management_Sys.Infrastructure.Identity
{
    public class UserActivityLog
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }   
        public Actions Action { get; set; } 
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public User user { get; set; }
    }
}
