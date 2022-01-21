using Data;
using Data.Models;
using WebApp.Models;

namespace WebApp.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly AppDbContext _context;

        public OrdersService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrdersSearchViewModel> SearchAsync(OrdersSearchConditionModel condition)
        {
            var searcher = new OrdersSearcher(_context);
            return await searcher.Search(condition);
        }

        public async Task<Order?> FindOrderAsync(int id)
        {
            var searcher = new OrdersSearcher(_context);
            return await searcher.FindOrderAsync(id);
        }
    }
}
