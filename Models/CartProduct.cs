namespace ShopSavvy.Models
{
    public class CartProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImagePath { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalValue { get; set;}
    }
}
