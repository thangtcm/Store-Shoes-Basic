using Microsoft.EntityFrameworkCore;
using StoreShoe.Data;
using StoreShoe.Models;
using StoreShoe.Repository.Base;

namespace StoreShoe.Services
{
    public class ProductService : EntityBaseRepository<Product>, IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductService(ApplicationDbContext context, IWebHostEnvironment hostEnvironment) : base(context)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task AddNewProductAsync(Product data, IFormFile upload)
        {
            if (upload != null && upload.Length > 0)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(upload.FileName);
                string extension = Path.GetExtension(upload.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                data.ImageName = fileName;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                data.ImagePath = path;
                using (var fileSrteam = new FileStream(path, FileMode.Create))
                {
                    await upload.CopyToAsync(fileSrteam);
                }
            }
            if (_context.Product != null)
            {
                await _context.Product.AddAsync(data);
                await _context.SaveChangesAsync();
            }
        }

        private Boolean DeleteImage(string imageName)
        {
            try
            {
                if(!String.IsNullOrEmpty(imageName))
                {
                    var filepath = Path.Combine(_hostEnvironment.WebRootPath + "/Image/", imageName);
                    if (File.Exists(filepath))
                        File.Delete(filepath);
                }    
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var ProductDetails = await _context!.Product!
                .FirstOrDefaultAsync(n => n.Id == id);
            return ProductDetails!;
        }

        public Task<List<Product>> GetProductOrderBy()
        {
            var ProductDetails = _context!.Product!
                .AsNoTracking().OrderBy(x => x.ProductName).ToListAsync();
            return ProductDetails;
        }
        public async Task UpdateProductAsync(Product data, IFormFile upload)
        {
            DeleteImage(data!.ImageName!);
            if (upload != null && upload.Length > 0)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(upload.FileName);
                string extension = Path.GetExtension(upload.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                data.ImageName = fileName;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                data.ImagePath = path;
                using (var fileSrteam = new FileStream(path, FileMode.Create))
                {
                    await upload.CopyToAsync(fileSrteam);
                }
            }
            _context.Update(data);
            await _context.SaveChangesAsync();
        }

        public Product GetById(int id)
        {
            return _context.Product!.FirstOrDefault(p => p.Id == id);
        }

        public bool Exists(int id)
        {
            return _context.Product!.Any(p => p.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public void Remove(Product data)
        {
            _context.Remove(data);
        }
    }
}
