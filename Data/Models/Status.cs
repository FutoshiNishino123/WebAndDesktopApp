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
        public int Id { get; protected set; }

        /// <summary>
        /// ステータス名
        /// </summary>
        [Required]
        [MaxLength(30)]
        [Display(Name = "ステータス名")]
        public string? Text { get; set; }
    }
}
