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

        private OrdersSearchConditionModel? GetCondition()
        {
            var json = HttpContext.Session.GetString("condition");
            if (json != null)
            {
                var condition = JsonSerializer.Deserialize<OrdersSearchConditionModel>(json);
                return condition;
            }
            return null;
        }

        private void SetCondition(OrdersSearchConditionModel condition)
        {
            var json = JsonSerializer.Serialize(condition);
            HttpContext.Session.SetString("condition", json);
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            if (page <= 0)
            {
                return View(new OrdersSearchViewModel());
            }

            var condition = GetCondition();
            if (condition != null)
            {
                condition.Page = page == null ? 1 : page.Value;
                var model = await _service.SearchAsync(condition);
                return View(model);
            }
            else
            {
                condition = new OrdersSearchConditionModel()
                {
                    Page = page == null ? 1 : page.Value,
                    Count = 100,
                };
                var model = await _service.SearchAsync(condition);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(OrdersSearchConditionModel condition)
        {
            condition.Page = 1;
            condition.Count = 100;
            SetCondition(condition);
            var model = await _service.SearchAsync(condition);
            return View(model);
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