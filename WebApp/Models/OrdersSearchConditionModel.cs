namespace WebApp.Models
{
    public class OrdersSearchConditionModel
    {
        /// <summary>
        /// 表示件数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// ページ数
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 検索番号
        /// </summary>
        public string? Number { get; set; }
    }
}
