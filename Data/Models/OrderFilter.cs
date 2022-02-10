namespace Data.Models
{
    public class OrderFilter
    {
        public string? Number { get; set; }

        public bool ActiveOnly { get; set; }

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
