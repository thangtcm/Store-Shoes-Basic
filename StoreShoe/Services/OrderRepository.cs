using StoreShoe.Data;
using StoreShoe.Models;
using System;

namespace StoreShoe.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(Order order, List<CartItem> cart)
        {
            order.OrderPlaced = DateTime.Now;
            order.Status = 0;
            decimal totalPrice = 0M;

            _context.Orders!.Add(order);
            await _context.SaveChangesAsync();

            foreach (var cartitem in cart!)
            {
                var product = _context.Product!.FirstOrDefault(x => x.Id == cartitem.product.Id);
                var orderDetail = new OrderDetail()
                {
                    Amount = cartitem.quantity,
                    ProductId = cartitem.product.Id,
                    size = cartitem.size,
                    OrderId = order.OrderID,
                    Price = cartitem.product.ProductPrice,
                };
                product!.Inventory -= cartitem.quantity;
                _context.Product!.Update(product);
                totalPrice += orderDetail.Price * orderDetail.Amount;
                _context.OrderDetails!.Add(orderDetail);
            }

            order.OrderTotal = totalPrice;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}
