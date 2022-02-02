using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public interface ITimeStamp
    {
        /// <summary>
        /// 作成日時
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// 更新日時
        /// </summary>
        public DateTime Updated { get; set; }
    }
}
