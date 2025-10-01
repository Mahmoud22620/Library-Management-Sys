using System.ComponentModel.DataAnnotations;

namespace Library_Management_Sys.Models.DTOs
{
    public class MemberDTO
    {
        public int MemberId { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Phone]
        public string Phone { get; set; }
    }
}
