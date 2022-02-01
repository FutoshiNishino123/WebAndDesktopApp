using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [MaxLength(100)]
        [Display(Name = "ID")]
        public string? Id { get; set; }

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
