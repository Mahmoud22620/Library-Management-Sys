using Library_Management_Sys.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace Library_Management_Sys.Models.DTOs
{
    public class BorrowTransactionDTO
    {
        public int Id { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public int MemberId { get; set; }
        [Required]
        public DateTime BorrowDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; }
        [Required]
        public BorrowTransStatus Status { get; set; } = BorrowTransStatus.Borrowed;

    }
}
