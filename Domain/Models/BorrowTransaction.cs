using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Management_Sys.Domain.Models
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
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        [Required]
        public string Status { get; set; }
        public virtual Book book { get; set; }
        public virtual Member member { get; set; }

        public virtual ICollection<BorrowTransaction> BorrowTransactions { get; set; }

    }
}
