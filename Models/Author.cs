using System.ComponentModel.DataAnnotations;

namespace Library_Management_Sys.Models
{
    public class Author
    {
        //(AuthorId, Name, Bio)
        [Key]
        public int AuthorId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Bio { get; set; }

        public virtual ICollection<Book> Books { get; set; }

    }
}
