using System.ComponentModel.DataAnnotations;

namespace Library_Management_Sys.Models
{
    public class Publisher
    {
        //(PublisherId, Name, Country, etc.)
        [Key]
        public int PublisherId { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Book> Books { get; set; }

    }
}
