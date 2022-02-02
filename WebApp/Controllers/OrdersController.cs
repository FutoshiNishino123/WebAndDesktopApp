using Data.Models;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrdersService _service;

        public OrdersController(IOrdersService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page, string? search)
        {
            if (page <= 0)
            {
                return NotFound();
            }

            var condition = new OrdersSearchCondition
            {
                Page = page ?? 1,
                Count = 100,
                SearchString = search,
                Filter = new OrderFilter
                {
                    Number = search,
                    ShowClosed = false,
                }
            };

            var result = await _service.SearchAsync(condition);

            var model = new OrdersViewModel
            {
                Result = result,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index([Bind("SearchString")] SearchCondition condition)
        {
            return RedirectToAction(nameof(Index), new { search = condition.SearchString });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var order = await _service.FindAsync(id.Value);
            if (order is null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}