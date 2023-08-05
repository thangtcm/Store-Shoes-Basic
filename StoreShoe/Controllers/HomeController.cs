using Microsoft.AspNetCore.Mvc;
using StoreShoe.Models;
using StoreShoe.Services;
using System.Diagnostics;
using X.PagedList;

namespace StoreShoe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _context;
        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _context= productService;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var allProduct = await _context.GetAllAsync();
            var sixproducr = allProduct.Take(6);
            return View(sixproducr);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}