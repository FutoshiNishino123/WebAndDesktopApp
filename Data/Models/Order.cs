using System.ComponentModel.DataAnnotations;

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
        public int Id { get; protected set; }

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
        /// アクティブ状態
        /// </summary>
        [Display(Name = "アクティブ状態")]
        public bool IsActive { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        [Required]
        [Display(Name = "作成")]
        public DateTime Created { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [Required]
        [Display(Name = "更新")]
        public DateTime Updated { get; set; }
    }
}
