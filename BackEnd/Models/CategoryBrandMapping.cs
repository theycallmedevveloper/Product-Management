using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class CategoryBrandMapping
    {
        [Key]
        public int Id { get; set; }

        public int Category_Id { get; set; }

        public int Brand_Id { get; set; }

        public bool IsActive { get; set; }
    }
}
