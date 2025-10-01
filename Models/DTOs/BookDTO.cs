using Library_Management_Sys.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Management_Sys.Models.DTOs
{
    public class BookDTO
    {
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

        public int CategoryId { get; set; }

        public int PublisherId { get; set; }

        public int AuthorId { get; set; }



    }
}
