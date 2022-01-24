using Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
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
        public async Task<IActionResult> Index(int? page, string? number)
        {
            var condition = new OrdersSearchConditionModel
            {
                Page = page ?? 1,
                Count = 100,
                Number = number,
            };
            var model = await _service.SearchAsync(condition);
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string? number)
        {
            return RedirectToAction(nameof(Index), new { number = number });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var order = await _service.FindOrderAsync(id.Value);
            if (order is null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}