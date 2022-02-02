using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Services
{
    public class OrdersSearch : IOrdersSearch
    {
        private readonly AppDbContext _context;

        public OrdersSearch(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrdersSearchResult> SearchAsync(SearchCondition condition)
        {
            if (condition.Page <= 0 || condition.Count <= 0)
            {
                throw new ArgumentException("検索条件が不正です。");
            }

            var query = from o in _context.Orders
                        select o;

            // TODO: より複雑な検索に対応できるようにする
            if (!string.IsNullOrEmpty(condition.SearchString))
            {
                query = query.Where(o => o.Number.Contains(condition.SearchString));
            }

            var total = await query.CountAsync();

            var index = (condition.Page - 1) * condition.Count;

            query = query.Include(o => o.User)
                         .Include(o => o.Status)
                         .OrderByDescending(o => o.Id)
                         .Skip(index)
                         .Take(condition.Count);

            var data = await query.ToListAsync();

            var pages = (int)Math.Ceiling(total / (double)condition.Count);

            var result = new OrdersSearchResult
            {
                Condition = condition,
                Total = total,
                Index = index,
                Pages = pages,
                Data = data
            };

            return result;
        }

        public async Task<Order?> FindAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Status)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order;
        }
    }
}