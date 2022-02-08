using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public interface ITimeStamp
    {
        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
}
