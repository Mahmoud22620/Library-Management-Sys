namespace Library_Management_Sys.Models.DTOs
{
    public class AuthDTO
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Role { get; set; }
        public string Token { get; set; }
        public DateTime ExpireOn { get; set; }
    }
}
