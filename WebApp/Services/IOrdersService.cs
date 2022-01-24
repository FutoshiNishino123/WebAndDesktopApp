using Data.Models;
using WebApp.Models;

namespace WebApp.Services
{
    public interface IOrdersService
    {
        Task<OrdersViewModel> SearchAsync(OrdersSearchConditionModel condition);
        Task<Order?> FindOrderAsync(int id);
    }
}
