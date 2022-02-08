using System.ComponentModel.DataAnnotations;

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
        public int Id { get; protected set; }

        /// <summary>
        /// アカウント
        /// </summary>
        [Required]
        [Display(Name = "アカウント")]
        public Account? Account { get; set; }

        /// <summary>
        /// ユーザ名
        /// </summary>
        [Required]
        [MaxLength(50)]
        [Display(Name = "ユーザ名")]
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
        /// 生年月日
        /// </summary>
        [Display(Name = "生年月日")]
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// 画像
        /// </summary>
        [MaxLength(1000)]
        [Display(Name = "画像")]
        public string? Image { get; set; }
    }
}
