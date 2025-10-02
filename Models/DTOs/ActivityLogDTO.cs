using Library_Management_Sys.Helpers.Enums;

namespace Library_Management_Sys.Models.DTOs
{
    public class ActivityLogDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public Permissions Action { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
