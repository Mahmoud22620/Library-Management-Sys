using System.ComponentModel.DataAnnotations;

namespace Library_Management_Sys.Models
{
    public class Member
    {
        //(MemberId, Name, Email, Phone, etc.)
        [Key]
        public int MemberId { get; set; }
        [Required]
        public string Name { get; set; }
        [Phone]
        public string Phone { get; set; }

        public virtual ICollection<BorrowTransaction> BorrowTransactions { get; set; }

    }
}
