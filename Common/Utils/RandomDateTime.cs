using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utils
{
    public class RandomDateTime
    {
        private readonly Random _random = new Random();
        private readonly DateTime _min;
        private readonly DateTime _max;

        public RandomDateTime(DateTime min, DateTime max)
        {
            _min = min;
            _max = max;
        }

        public DateTime Next()
        {
            var span = _max - _min;
            var seconds = _random.Next((int)span.TotalSeconds);
            var result = _min.AddSeconds(seconds);
            return result;
        }
    }
}
