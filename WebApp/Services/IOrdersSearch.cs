using Data.Models;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IOrdersSearch
    {
        Task<OrdersSearchResult> SearchAsync(OrdersSearchCondition condition);
        Task<Order?> FindAsync(int id);
    }
}