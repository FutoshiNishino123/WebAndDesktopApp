using Data;
using Data.Models;
using WebApp.Models;

namespace WebApp.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersSearch _search;

        public OrdersService(IOrdersSearch search)
        {
            _search = search;
        }

        public async Task<OrdersSearchResult> SearchAsync(OrdersSearchCondition condition)
        {
            return await _search.SearchAsync(condition);
        }

        public async Task<Order?> FindAsync(int id)
        {
            return await _search.FindAsync(id);
        }
    }
}
