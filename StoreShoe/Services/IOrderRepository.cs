using StoreShoe.Models;

namespace StoreShoe.Services
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order, List<CartItem> cart);
    }
}
