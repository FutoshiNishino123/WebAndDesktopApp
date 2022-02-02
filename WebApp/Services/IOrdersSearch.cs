using Data.Models;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IOrdersSearch
    {
        Task<OrdersSearchResult> SearchAsync(SearchCondition condition);
        Task<Order?> FindAsync(int id);
    }
}