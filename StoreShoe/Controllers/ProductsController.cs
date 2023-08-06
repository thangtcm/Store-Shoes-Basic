using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StoreShoe.Data;
using StoreShoe.Migrations;
using StoreShoe.Models;
using StoreShoe.Services;
using X.PagedList;

namespace StoreShoe.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _context;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProductsController(IProductService context, UserManager<ApplicationUser> userManager, IOrderRepository orderRepository)
        {
            _context = context;
            _userManager = userManager;
            _orderRepository = orderRepository;
        }
        public const string CARTKEY = "cart";
        // GET: Products
        public async Task<IActionResult> Index(int? page)
        {
            int pagesize = 6;
            int pagenumber = page == null || page < 0 ? 1 : page.Value;
            var allProduct = await _context.GetProductOrderBy();
            PagedList<Product> lst = new PagedList<Product>(allProduct, pagenumber, pagesize);
            return View(lst);
        }

        List<CartItem> GetCartItems()
        {

            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }

        [Route("addcart/{productid:int}", Name = "addcart")]
        public IActionResult AddToCart([FromRoute] int productid, int quantity = 1, int size = 35)//size mặc định 35
        {

            var product = _context.GetById(productid);
            if (product == null)
                return NotFound("Không có sản phẩm");

            // Xử lý đưa vào Cart ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.Id == productid);
            if (cartitem != null)
            {
                cartitem.quantity += quantity;
            }
            else
            {
                cart.Add(new CartItem() { quantity = quantity, product = product, size = size});
            }

            SaveCartSession(cart);
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Checkout(Order order)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            order.UserId = userId;

            var cart = GetCartItems();

            if (cart.Count == 0)
            {
                ModelState.AddModelError("", "Giỏ hàng trống");
            }

            if (ModelState.IsValid)
            {
                await _orderRepository.CreateOrderAsync(order, cart);
                ClearCart();
                return RedirectToAction("CheckoutComplete");
            }

            return View(order);
        }

        [Authorize]
        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = $"Cảm ơn bạn đã đặt hàng, chúng tôi sẽ giao hàng cho bạn sớm nhất!";
            return View();
        }

        [Route("/cart", Name = "cart")]
        public IActionResult Cart()
        {
            return View(GetCartItems());
        }

        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart([FromForm] int productid, [FromForm] int quantity)
        {
            // Cập nhật Cart thay đổi số lượng quantity ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.Id == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cartitem.quantity = quantity;
            }
            SaveCartSession(cart);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }

        [Route("/removecart/{productid:int}", Name = "removecart")]
        public IActionResult RemoveCart([FromRoute] int productid)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.product.Id == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cart.Remove(cartitem);
            }

            SaveCartSession(cart);
            return RedirectToAction(nameof(Index));
        }

        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        void SaveCartSession(List<CartItem> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var productDetail = await _context.GetProductByIdAsync(id);

            return View(productDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, int size, int quantity)
        {
            var product = await _context.GetProductByIdAsync(id);
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("AddToCart", "Products", new { productid = product.Id, quantity = quantity, size = size });
            }
            return View(product);
        }
    }
}
