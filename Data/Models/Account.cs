using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    /// <summary>
    /// アカウント
    /// </summary>
    public class Account
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [Display(Name = "ID")]
        public int Id { get; protected set; }

        /// <summary>
        /// アカウント名
        /// </summary>
        [Required]
        [MaxLength(100)]
        [Display(Name = "アカウント名")]
        public string? Name { get; set; }

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
    }
}
