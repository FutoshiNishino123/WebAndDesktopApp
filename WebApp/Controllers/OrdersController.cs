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

        public int DisplayItemsCount { get; set; } = 100;

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
                return RedirectToAction("Index", 1);
            }

            var orders = await _context.Orders
                .Include(o => o.Person)
                .Include(o => o.Status)
                .OrderByDescending(o => o.Id)
                .Skip((page.Value - 1) * DisplayItemsCount)
                .Take(DisplayItemsCount)
                .ToListAsync();

            var count = await _context.Orders.CountAsync();

            ViewBag.Page = page.Value;
            ViewBag.Count = count;

            var model = orders;
            return View(model);
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

            var model = order;

            return View(model);
        }
    }
}