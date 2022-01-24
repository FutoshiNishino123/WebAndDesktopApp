using Data.Models;

namespace WebApp.Models
{
    public class OrdersViewModel : DataListViewModel<Order>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        public OrdersSearchConditionModel? Condition { get; set; }
    }
}
