using Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrdersSearcher _searcher;

        public OrdersController(AppDbContext context)
        {
            _searcher = new OrdersSearcher(context);
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? search, int? page)
        {
            if (page is null)
            {
                page = 1;
            }

            if (page <= 0)
            {
                return RedirectToAction("Index", new { search = search });
            }

            var model = await _searcher.FindOrdersAsync(page.Value, search);

            // todo: 消したい => ViewModelの値を利用できれば良し
            ViewBag.Search = search;

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string search)
        {
            return RedirectToAction("Index", new { search = search });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var order = await _searcher.FindOrderAsync(id.Value);

            if (order is null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}