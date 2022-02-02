namespace Data.Models
{
    public class OrderFilter
    {
        public string? Number { get; set; }

        public bool ShowClosed { get; set; }

        public bool Apply(Order order)
        {
            if (!ShowClosed && order.IsClosed)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(Number)
                && (!string.IsNullOrEmpty(order.Number) && !order.Number.Contains(Number)))
            {
                return false;
            }

            return true;
        }
    }
}
