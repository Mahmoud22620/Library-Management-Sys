using Library_Management_Sys.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Management_Sys.Models
{
    public class BorrowTransaction
    {
        //(Id, BookId, MemberId, BorrowDate, ReturnDate, Status)
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("book")]
        public int BookId { get; set; }
        [Required]
        [ForeignKey("member")]
        public int MemberId { get; set; }
        [Required]
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; }
        [Required]
        public BorrowTransStatus Status { get; set; } = BorrowTransStatus.Borrowed;
        public virtual Book book { get; set; }
        public virtual Member member { get; set; }
    }
}
