using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public interface ITimeStamp
    {
        /// <summary>
        /// 作成日時
        /// </summary>
        [Display(Name = "作成")]
        public DateTime Created { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        [Display(Name = "更新")]
        public DateTime Updated { get; set; }
    }
}
