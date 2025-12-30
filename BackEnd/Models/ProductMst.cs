using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Models
{
    public class ProductMst
    {
        [Key]
        public int Product_Id { get; set; }

        public string Product_Name { get; set; }

        public string Product_Code { get; set; }

        public int Category_Id { get; set; }

        public int Brand_Id { get; set; }

        public decimal Price { get; set; }

        [Column(TypeName = "date")]
        public DateOnly Mfg_Date { get; set; }

        [Column(TypeName = "date")]
        public DateOnly? Expiry_Date { get; set; }

        public DateTime Entry_Date { get; set; }

        public DateTime Update_Date { get; set; }

        public bool IsActive { get; set; }
    }
}
