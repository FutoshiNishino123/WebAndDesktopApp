namespace WebApp.Models
{
    public class OrdersViewModel
    {
        /// <summary>
        /// 検索結果
        /// </summary>
        public OrdersSearchResult? Result { get; set; }

        /// <summary>
        /// ページ数
        /// </summary>
        public string? Pages
        {
            get
            {
                if (Result is null
                    || Result.Pages == 0)
                {
                    return "1";
                }
                return Result.Pages.ToString();
            }
        }

        /// <summary>
        /// ページ
        /// </summary>
        public string? Page
        {
            get
            {
                if (Result is null
                    || Result.Condition is null)
                {
                    return "1";
                }
                return Result.Condition.Page.ToString();
            }
        }

        /// <summary>
        /// 次ページ
        /// </summary>
        public string? NextPage
        {
            get
            {
                if (Result is null
                    || Result.Condition is null)
                {
                    return "1";
                }
                return (Result.Condition.Page + 1).ToString();
            }
        }

        /// <summary>
        /// 前ページ
        /// </summary>
        public string? PrevPage
        {
            get
            {
                if (Result is null
                    || Result.Condition is null
                    || Result.Condition.Page <= 1)
                {
                    return "1";
                }
                return (Result.Condition.Page - 1).ToString();
            }
        }

        /// <summary>
        /// ヒット件数
        /// </summary>
        public string? Total
        {
            get
            {
                if (Result is null)
                {
                    return "0";
                }
                return Result.Total.ToString("#,#0");
            }
        }

        /// <summary>
        /// 開始位置
        /// </summary>
        public string? Start
        {
            get
            {
                if (Result is null
                    || Result.Data is null
                    || Result.Data.Count == 0)
                {
                    return "0";
                }
                return (Result.Index + 1).ToString("#,#0");
            }
        }

        /// <summary>
        /// 終了位置
        /// </summary>
        public string? End
        {
            get
            {
                if (Result is null
                    || Result.Data is null
                    || Result.Data.Count == 0)
                {
                    return "0";
                }
                return (Result.Index + Result.Data.Count).ToString("#,#0");
            }
        }
    }
}
