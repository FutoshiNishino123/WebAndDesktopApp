namespace WebApp.Models
{
    public class SearchCondition
    {
        /// <summary>
        /// 件数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// ページ
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 検索文字列
        /// </summary>
        public string? SearchString { get; set; }
    }
}
