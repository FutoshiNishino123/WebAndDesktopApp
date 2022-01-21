namespace WebApp.Models
{
    public class SearchResultViewModel<T>
    {
        public int Page { get; set; }
        public int Total { get; set; }
        public List<T>? Data { get; set; }
        public int FirstIndex { get; set; }
        public int LastIndex { get; set; }
    }
}
