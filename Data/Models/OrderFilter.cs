namespace Data.Models
{
    public class OrderFilter
    {
        public string? Number { get; set; }

        public bool ShowClosed { get; set; }

        public IQueryable<Order> Apply(IQueryable<Order> query)
        {
            if (!string.IsNullOrWhiteSpace(Number))
            {
                query = query.Where(o => o.Number.Contains(Number));
            }

            if (!ShowClosed)
            {
                query = query.Where(o => o.IsClosed == false);
            }

            return query;
        }
    }
}
