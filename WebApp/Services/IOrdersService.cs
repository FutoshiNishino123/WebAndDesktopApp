using Data.Models;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IOrdersService
    {
        Task<OrdersSearchViewModel> SearchAsync(OrdersSearchConditionModel condition);
        Task<Order?> FindOrderAsync(int id);
    }
}
