using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;

        public int DisplayCount { get; set; } = 100;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            if (page == null)
            {
                page = 1;
            }

            if (page <= 0)
            {
                return RedirectToAction("Index", null);
            }

            var skipCount = (page.Value - 1) * DisplayCount;

            var orders = await _context.Orders
                .Include(o => o.Person)
                .Include(o => o.Status)
                .OrderByDescending(o => o.Id)
                .Skip(skipCount)
                .Take(DisplayCount)
                .ToListAsync();

            var total = await _context.Orders.CountAsync();

            var firstIndex = orders.Any() ? skipCount + 1 : 0;
            var lastIndex = orders.Any() ? skipCount + orders.Count : 0;

            ViewBag.Page = page.Value;
            ViewBag.Total = total;
            ViewBag.Count = orders.Count;
            ViewBag.FirstIndex = firstIndex;
            ViewBag.LastIndex = lastIndex;

            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Person)
                .Include(o => o.Status)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order is null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}