using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    /// <summary>
    /// ステータス
    /// </summary>
    public class Status
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        [Required]
        [MaxLength(30)]
        [Display(Name = "名前")]
        public string? Text { get; set; }
    }
}
