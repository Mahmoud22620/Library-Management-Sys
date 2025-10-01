using System.ComponentModel.DataAnnotations;

namespace Library_Management_Sys.Models
{
    public class Category
    {
        //(CategoryId, Name, ParentCategoryId nullable)
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; }

        public virtual ICollection<Book> Books { get; set; }

    }
}
