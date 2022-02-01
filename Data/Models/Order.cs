using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    /// <summary>
    /// 指図伝票
    /// </summary>
    public class Order : ITimeStamp
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        /// <summary>
        /// 番号
        /// </summary>
        [Required]
        [MaxLength(30)]
        [Display(Name = "番号")]
        public string? Number { get; set; }

        /// <summary>
        /// 担当者
        /// </summary>
        [Display(Name = "担当者")]
        public User? User { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        [Display(Name = "ステータス")]
        public Status? Status { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [MaxLength(300)]
        [Display(Name = "備考")]
        public string? Remarks { get; set; }

        /// <summary>
        /// 期限
        /// </summary>
        [Display(Name = "期限")]
        public DateTime? Expiration { get; set; }

        /// <summary>
        /// クローズ
        /// </summary>
        [Display(Name = "クローズ")]
        public bool IsClosed { get; set; }

        #region ITimeStamp
        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
        #endregion
    }
}
