namespace WebApp.Models
{
    public class SearchResult<T>
    {
        /// <summary>
        /// 検索条件
        /// </summary>
        public SearchCondition? Condition { get; set; }

        /// <summary>
        /// ヒット件数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// ページ数
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// 開始位置
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 取得データ
        /// </summary>
        public ICollection<T>? Data { get; set; }
    }
}