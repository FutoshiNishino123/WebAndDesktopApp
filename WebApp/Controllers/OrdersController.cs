﻿using Data;
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

            var orders = await _context.Orders
                .Include(o => o.Person)
                .Include(o => o.Status)
                .OrderBy(o => o.UpdatedDate)
                .Skip((page.Value - 1) * DisplayItemsCount)
                .Take(DisplayItemsCount)
                .ToListAsync();

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