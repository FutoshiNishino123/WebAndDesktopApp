using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Models
{
    public class OrdersSearcher
    {
        private readonly AppDbContext _context;

        public OrdersSearcher(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrdersSearchViewModel> Search(OrdersSearchConditionModel condition)
        {
            if (condition.Page <= 0)
            {
                throw new ArgumentException("Page must be greater than 0.", nameof(condition.Page));
            }

            if (condition.Count <= 0)
            {
                throw new ArgumentException("Count must be greater than 0.", nameof(condition.Count));
            }

            var query = from o in _context.Orders
                        select o;

            if (!string.IsNullOrEmpty(condition.Number))
            {
                query = query.Where(o => o.Number.Contains(condition.Number));
            }

            var total = await query.CountAsync();

            var skipCount = (condition.Page - 1) * condition.Count;
            query = query.Include(o => o.Person)
                         .Include(o => o.Status)
                         .OrderByDescending(o => o.Id)
                         .Skip(skipCount)
                         .Take(condition.Count);

            var orders = await query.ToListAsync();
            var firstIndex = orders.Any() ? skipCount + 1 : 0;
            var lastIndex = orders.Any() ? skipCount + orders.Count : 0;

            var model = new OrdersSearchViewModel
            {
                Condition = condition,
                Data = orders,
                Page = condition.Page,
                Total = total,
                FirstIndex = firstIndex,
                LastIndex = lastIndex,
            };

            return model;
        }

        public async Task<Order?> FindOrderAsync(int id)
        {
            var query = from o in _context.Orders
                        select o;

            query = query.Include(o => o.Person)
                         .Include(o => o.Status);

            var order = await query.FirstOrDefaultAsync(o => o.Id == id);

            return order;
        }
    }
}