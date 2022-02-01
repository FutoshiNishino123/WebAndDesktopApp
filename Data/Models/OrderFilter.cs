namespace Data.Models
{
    public class OrderFilter
    {
        public string? KeyWord { get; set; }

        public bool ShowClosed { get; set; }

        public bool Apply(Order order)
        {
            if (!ShowClosed && order.IsClosed)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(KeyWord)
                && (!string.IsNullOrEmpty(order.Number) && !order.Number.Contains(KeyWord)))
            {
                return false;
            }

            return true;
        }
    }
}
