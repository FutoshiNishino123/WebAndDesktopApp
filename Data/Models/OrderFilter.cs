namespace Data.Models
{
    public class OrderFilter
    {
        public string? Number { get; set; }

        public bool ActiveOnly { get; set; }

        public bool Apply(Order order)
        {
            if (!string.IsNullOrWhiteSpace(Number) && !order.Number.Contains(Number))
            {
                return false;
            }

            if (ActiveOnly && !order.IsActive)
            {
                return false;
            }

            return true;
        }

        public IQueryable<Order> Apply(IQueryable<Order> query)
        {
            if (!string.IsNullOrWhiteSpace(Number))
            {
                query = query.Where(o => o.Number.Contains(Number));
            }

            if (ActiveOnly)
            {
                query = query.Where(o => o.IsActive == true);
            }

            return query;
        }
    }
}
