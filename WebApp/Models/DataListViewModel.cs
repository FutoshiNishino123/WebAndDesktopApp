namespace WebApp.Models
{
    public class DataListViewModel<T>
    {
        /// <summary>
        /// 表示ページ
        /// </summary>
        public string? Page { get; set; }

        /// <summary>
        /// 次ページ
        /// </summary>
        public string? NextPage { get; set; }

        /// <summary>
        /// 前ページ
        /// </summary>
        public string? PrevPage { get; set; }

        /// <summary>
        /// 全データ数
        /// </summary>
        public string? Total { get; set; }

        /// <summary>
        /// 表示データ
        /// </summary>
        public List<T>? Data { get; set; }

        /// <summary>
        /// 表示データの先頭インデックス
        /// </summary>
        public string? FirstIndex { get; set; }

        /// <summary>
        /// 表示データの末尾インデックス
        /// </summary>
        public string? LastIndex { get; set; }
    }
}
