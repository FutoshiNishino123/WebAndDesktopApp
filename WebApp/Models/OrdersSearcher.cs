using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Models
{
    public class OrdersSearcher
    {
        private readonly AppDbContext _context;

        public int PageItemsCount { get; set; } = 100;

        public OrdersSearcher(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrdersViewModel> FindOrdersAsync(int page, string? search)
        {
            var query = from o in _context.Orders
                        select o;

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o => o.Number.Contains(search));
            }
            
            var itemsCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)itemsCount / PageItemsCount);
            
            var skipCount = (page - 1) * PageItemsCount;

            query = query.Include(o => o.Person)
                         .Include(o => o.Status)
                         .OrderByDescending(o => o.Id)
                         .Skip(skipCount)
                         .Take(PageItemsCount);

            var orders = await query.ToListAsync();

            var firstIndex = orders.Any() ? skipCount + 1 : 0;
            var lastIndex = orders.Any() ? skipCount + orders.Count : 0;

            var model = new OrdersViewModel
            {
                Orders = orders,
                Search = search,
                TotalPages = totalPages,
                Page = page,
                PrevPage = page - 1,
                NextPage = page + 1,
                ItemsCount = itemsCount.ToString("#,#0"),
                FirstIndex = firstIndex.ToString("#,#0"),
                LastIndex = lastIndex.ToString("#,#0"),
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