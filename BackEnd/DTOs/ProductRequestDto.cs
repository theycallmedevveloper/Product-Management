namespace ProductManagement.DTOs
{
    public class ProductRequestDto
    {
        public int Product_Id { get; set; }
        public string Product_Name { get; set; }
        public string Product_Code { get; set; }
        public int Category_Id { get; set; }
        public int Brand_Id { get; set; }
        public decimal Price { get; set; }
        public string Mfg_Date { get; set; }
        public string? Expiry_Date { get; set; }
    }
}
