using Data.Models;

namespace WebApp.Models
{
    public class OrdersSearchViewModel : SearchResultViewModel<Order>
    {
        public OrdersSearchConditionModel? Condition { get; set; }
    }
}
