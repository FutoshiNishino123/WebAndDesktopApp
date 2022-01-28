using Data.Models;

namespace PrismApp.ViewModels
{
    public class OrderFilter
    {
        public bool ShowClosed { get; set; }

        public bool Filter(Order order)
        {
            if (!ShowClosed && order.IsClosed)
            {
                return false;
            }

            return true;
        }
    }
}
