using Data.Models;

namespace WebApp.Models
{
    public class OrdersSearchCondition : SearchCondition
    {
        public OrderFilter? Filter { get; set; }
    }
}