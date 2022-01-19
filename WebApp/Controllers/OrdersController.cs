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

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _context.Orders.Include(o => o.Person)
                                             .Include(o => o.Status)
                                             .ToListAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var model = await _context.Orders.Include(o => o.Person)
                                             .Include(o => o.Status)
                                             .FirstOrDefaultAsync(o => o.Id == id);
            if (model is null)
            {
                return NotFound();
            }

            return View(model);
        }
    }
}