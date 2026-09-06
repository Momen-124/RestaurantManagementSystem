using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Data;
using RestaurantManagementSystem.Models;
using System.Diagnostics;

namespace RestaurantManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Dashboard()
        {
            var totalOrders = await _context.Orders.CountAsync();
            var totalRevenue = await _context.OrderItems.SumAsync(oi => oi.Quantity * oi.MenuItem.Price);
            var pendingOrders = await _context.Orders.CountAsync(o => o.Status == "Pending");

            var topMenuItem = await _context.OrderItems
                .GroupBy(oi => oi.MenuItem.Name)
                .Select(g => new { Name = g.Key, TotalQuantity = g.Sum(oi => oi.Quantity) })
                .OrderByDescending(x => x.TotalQuantity)
                .FirstOrDefaultAsync();

            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.PendingOrders = pendingOrders;
            ViewBag.TopMenuItem = topMenuItem?.Name ?? "لا يوجد";
            ViewBag.TopMenuItemQty = topMenuItem?.TotalQuantity ?? 0;

            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reports()
        {
            // تقرير حسب الصنف
            var salesByItem = await _context.OrderItems
                .GroupBy(oi => oi.MenuItem.Name)
                .Select(g => new
                {
                    ItemName = g.Key,
                    TotalQuantity = g.Sum(oi => oi.Quantity),
                    TotalRevenue = g.Sum(oi => oi.Quantity * oi.MenuItem.Price)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .ToListAsync();

            // تقرير حسب اليوم
            var salesByDay = await _context.Orders
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    OrderCount = g.Count(),
                    DayRevenue = g.SelectMany(o => o.OrderItems)
                                  .Sum(oi => oi.Quantity * oi.MenuItem.Price)
                })
                .OrderByDescending(x => x.Date)
                .ToListAsync();

            ViewBag.SalesByItem = salesByItem;
            ViewBag.SalesByDay = salesByDay;

            return View();
        }
    }
}
