using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    /// <summary>
    /// 性別
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// 不明
        /// </summary>
        Unknown,

        /// <summary>
        /// 男性
        /// </summary>
        Male,

        /// <summary>
        /// 女性
        /// </summary>
        Female,

        /// <summary>
        /// その他
        /// </summary>
        Other,
    }
}
