using System.ComponentModel.DataAnnotations;

namespace Library_Management_Sys.Models.DTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
