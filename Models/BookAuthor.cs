using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Management_Sys.Models
{
    public class BookAuthor
    {
        [ForeignKey("Book")]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }
    }
}
