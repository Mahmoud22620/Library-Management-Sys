using Library_Management_Sys.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace Library_Management_Sys.Models
{
    public class User : IdentityUser<Guid>
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Permissions User_permissions { get; set; } 
        public ICollection<UserActivityLog> ActivityLogs { get; set; }



    }
}
