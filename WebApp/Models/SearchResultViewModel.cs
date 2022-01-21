namespace WebApp.Models
{
    public class SearchResultViewModel<T>
    {
        /// <summary>
        /// 現在のページ
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 検索結果の総データ数（表示・非表示を含む）
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 検索結果のデータ
        /// </summary>
        public List<T>? Data { get; set; }

        /// <summary>
        /// データの先頭インデックス
        /// </summary>
        public int FirstIndex { get; set; }

        /// <summary>
        /// データの末尾インデックス
        /// </summary>
        public int LastIndex { get; set; }
    }
}
