using Microsoft.AspNetCore.Identity;

namespace Library_Management_Sys.Infrastructure.Identity
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserActivityLog> ActivityLogs { get; set; }
    }
}
