using StoreShoe.Models;
using StoreShoe.Repository.Base;
using StoreShoe.ViewModels;

namespace StoreShoe.Services
{
    public interface IProductService : IEntityBaseRepository<Product>
    {
        Task<Product> GetProductByIdAsync(int id);

        Task<List<Product>> GetProductOrderBy();

        Task AddNewProductAsync(Product data, IFormFile upload);

        Task UpdateProductAsync(Product data, IFormFile upload);
        Product GetById(int id);
        void Remove(Product data);

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
