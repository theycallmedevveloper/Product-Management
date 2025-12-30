using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Models
{
    public class BrandMst
    {
        [Key]
        public int Brand_Id { get; set; }

        public string Brand_Name { get; set; }

        public bool IsActive { get; set; }
    }
}
    