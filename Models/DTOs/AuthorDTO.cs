using System.ComponentModel.DataAnnotations;

namespace Library_Management_Sys.Models.DTOs
{
    public class AuthorDTO
    {
        public int AuthorId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Bio { get; set; }
    }
}
