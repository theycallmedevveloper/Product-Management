namespace ProductManagement.DTOs
{
    public class ProductResponseDto
    {
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Product_Code { get; set; }
        public decimal Price { get; set; }
        public string Mfg_Date { get; set; }
        public string? Expiry_Date { get; set; }

        public CategoryDto Category { get; set; }
        public BrandDto Brand { get; set; }
    }
}
