using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    /// <summary>
    /// ユーザ
    /// </summary>
    public class User
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        /// <summary>
        /// メールアドレス
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Display(Name = "メールアドレス")]
        public string? EmailAddress { get; set; }

        /// <summary>
        /// パスワード（ハッシュ値）
        /// </summary>
        [Required]
        [MaxLength(100)]
        [Display(Name = "パスワード")]
        public string? Password { get; set; }

        /// <summary>
        /// 管理者
        /// </summary>
        [Display(Name = "管理者")]
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Display(Name = "名前")]
        public string? Name { get; set; }

        /// <summary>
        /// フリガナ
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Display(Name = "フリガナ")]
        public string? Kana { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        [Display(Name = "性別")]
        public Gender Gender { get; set; }

        /// <summary>
        /// 画像
        /// </summary>
        [MaxLength(1000)]
        [Display(Name = "画像")]
        public string? Image { get; set; }
    }
}
