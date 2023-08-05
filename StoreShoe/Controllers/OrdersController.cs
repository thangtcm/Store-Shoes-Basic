using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using StoreShoe.Core;
using StoreShoe.Data;
using StoreShoe.Models;
using StoreShoe.Models.Enum;
using StoreShoe.Services;
using System.Threading.Tasks;

namespace StoreShoe.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrdersController(IOrderRepository orderRepository,
             ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _orderRepository = orderRepository;
            _context = context;
            _userManager = userManager;
        }

        // GET: Reviews
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            bool isAdmin = await _userManager.IsInRoleAsync(user, Constants.Roles.Admin);

            if (isAdmin)
            {
                var allOrders = await _context.Orders!.Include(o => o.OrderLines).Include(o => o.User).ToListAsync();
                return View(allOrders);
            }
            else
            {
                var orders = await _context.Orders!.Include(o => o.OrderLines).Include(o => o.User)
                    .Where(r => r.User == user).ToListAsync();
                return View(orders);
            }
        }

        // GET: Orders/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var orders = await _context.Orders!.Include(o => o.OrderLines).Include(o => o.User)
                .SingleOrDefaultAsync(m => m.OrderID == id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userRoles = await _userManager.GetRolesAsync(user);
            bool isAdmin = userRoles.Any(r => r == Constants.Roles.Admin);

            if (orders == null)
            {
                return NotFound();
            }

            if (isAdmin == false)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                if (orders.UserId != userId)
                {
                    return BadRequest("You do not have permissions to view this order.");
                }
            }

            var orderDetailsList = _context.OrderDetails!.Include(o => o.Product!).Include(o => o.Order)
                .Where(x => x.OrderId == orders.OrderID);

            ViewBag.OrderDetailsList = orderDetailsList;
            var enumData = from StatusOrder e in Enum.GetValues(typeof(StatusOrder))
                           select new
                           {
                               ID = (int)e,
                               Name = e.ToString()
                           };
            ViewBag.StatusOrder = new SelectList(enumData, "ID", "Name", orders.Status);

            return View(orders);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, [Bind("OrderID, Status")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            try
            {
                var getorder = await _context.Orders.FirstOrDefaultAsync(x => x.OrderID == id);
                if(getorder == null)
                {
                    return NotFound();
                }
                getorder.Status = order.Status;
                _context.Orders!.Update(getorder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Orders/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = Constants.Roles.Admin)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders!.Include(o => o.User)
                .SingleOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: OrdersTest/Delete/5
        [Authorize(Roles = Constants.Roles.Admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders!.SingleOrDefaultAsync(m => m.OrderID == id);
            _context.Orders!.Remove(order!);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
