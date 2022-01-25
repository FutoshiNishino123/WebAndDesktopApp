using Data;
using Data.Models;
using WebApp.Models;

namespace WebApp.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly OrdersSearcher _searcher;

        public OrdersService(AppDbContext context)
        {
            _searcher = new OrdersSearcher(context);
        }

        public async Task<OrdersViewModel> SearchAsync(OrdersSearchConditionModel condition)
        {
            return await _searcher.SearchOrdersAsync(condition);
        }

        public async Task<Order?> FindOrderAsync(int id)
        {
            return await _searcher.FindOrderAsync(id);
        }
    }
}
