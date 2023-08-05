namespace StoreShoe.Models
{
    public class CartItem
    {
        public int quantity { set; get; }
        public int size { set; get; }
        public Product product { set; get; }
    }
}
