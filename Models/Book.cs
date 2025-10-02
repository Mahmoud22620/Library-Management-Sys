using Library_Management_Sys.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Management_Sys.Models
{
    public class Book
    {
        //(BookId, Title, ISBN, Edition, Year, Summary, CoverImage, Status, CategoryId, PublisherId,language)

        [Key]
        public int BookId { get; set; }
        [Required]
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Edition { get; set; }
        public int Year { get; set; }
        public string Summary { get; set; }
        public string CoverImage { get; set; }

        public string Language { get; set; }
        public BookStatus Status { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }


        [ForeignKey("Publisher")]
        public int PublisherId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Publisher Publisher { get; set; }

        public virtual ICollection<BookAuthor> Authors { get; set; }

        public virtual ICollection<BorrowTransaction> BorrowTransactions { get; set; } 
        


        }
}
