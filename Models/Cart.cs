namespace ShopSavvy.Models
{
    public class Cart
    {
        public List<CartProduct> Products { get; set; } = new List<CartProduct>();

        public void AddItem(Product product,int quantity)
        {
            var existingItem = Products.FirstOrDefault(item => item.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.TotalValue = existingItem.Price*quantity;
            }
            else
            {
                Products.Add(new CartProduct
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    TotalValue=product.Price*quantity,
                    ProductImagePath=product.ImagePath
                });
            }
        }

        public void UpdateItem(int product_id, int quantity)
        {
            var existingItem = Products.FirstOrDefault(item => item.ProductId == product_id);
            if (existingItem != null)
            {
                existingItem.Quantity = quantity;
                existingItem.TotalValue = existingItem.Price * quantity;
            }
        }

        public void RemoveItem(int productId)
        {
            var existingItem = Products.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem != null)
            {
                Products.Remove(existingItem);
            }
        }

        public decimal GetTotal()
        {
            return Products.Sum(item => item.Price * item.Quantity);
        }

        public void Clear()
        {
            Products.Clear();
        }
    }
}
