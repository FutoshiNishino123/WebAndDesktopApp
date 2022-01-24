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

        public async Task<OrdersViewModel> Search(OrdersSearchConditionModel condition)
        {
            if (condition.Page <= 0 || condition.Count <= 0)
            {
                return new OrdersViewModel();
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
            var page = condition.Page;
            var nextPage = page + 1;
            var prevPage = page - 1;

            var model = new OrdersViewModel
            {
                Condition = condition,
                Data = orders,
                Page = page.ToString(),
                NextPage = nextPage.ToString(),
                PrevPage = prevPage.ToString(),
                Total = total.ToString("#,#0"),
                FirstIndex = firstIndex.ToString("#,#0"),
                LastIndex = lastIndex.ToString("#,#0"),
            };

            return model;
        }

        public async Task<Order?> FindOrderAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Person)
                .Include(o => o.Status)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order;
        }
    }
}