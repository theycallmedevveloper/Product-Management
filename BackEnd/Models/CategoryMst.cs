using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class CategoryMst
    {
        [Key]
        public int Category_Id { get; set; }

        public string Category_Name { get; set; }

        public int? Parent_Category_Id { get; set; }
        public bool IsActive{ get; set; }
    }
}
