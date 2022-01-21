using Data.Models;

namespace WebApp.Models
{
    public class OrdersViewModel
    {
        public IEnumerable<Order>? Orders { get; set; }
        public string? Search { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public int PrevPage { get; set; }
        public int NextPage { get; set; }
        public string? ItemsCount { get; set; }
        public string? FirstIndex { get; set; }
        public string? LastIndex { get; set; }
    }
}
